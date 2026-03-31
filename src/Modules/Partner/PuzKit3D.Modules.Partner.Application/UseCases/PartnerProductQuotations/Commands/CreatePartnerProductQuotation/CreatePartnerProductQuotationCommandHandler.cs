using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.Services;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.CreatePartnerProductQuotation;

internal sealed class CreatePartnerProductQuotationCommandHandler : ICommandTHandler<CreatePartnerProductQuotationCommand, Guid>
{
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerProductRequestRepository _requestRepository;
    private readonly IPartnerProductQuotationDetailRepository _detailRepository;
    private readonly IImportServiceConfigRepository _importServiceConfigRepository;
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IPartnerProductQuotationCodeGenerator _codeGenerator;

    public CreatePartnerProductQuotationCommandHandler(
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerProductRequestRepository requestRepository,
        IPartnerProductQuotationDetailRepository detailRepository,
        IImportServiceConfigRepository importServiceConfigRepository,
        IPartnerRepository partnerRepository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductQuotationCodeGenerator codeGenerator)
    {
        _quotationRepository = quotationRepository;
        _requestRepository = requestRepository;
        _detailRepository = detailRepository;
        _importServiceConfigRepository = importServiceConfigRepository;
        _partnerRepository = partnerRepository;
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
    }

    public async Task<ResultT<Guid>> Handle(
        CreatePartnerProductQuotationCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            // kiểm tra request
            var existingRequest = await _requestRepository.GetDetailByIdAsync(
                PartnerProductRequestId.From(request.PartnerProductRequestId),
                cancellationToken);

            if (existingRequest == null)
            {
                return Result.Failure<Guid>(PartnerProductRequestError.NotFound(request.PartnerProductRequestId));
            }

            // kiểm tra quotation
            var existingQuotation = await _quotationRepository.GetByRequestIdAsync(
                PartnerProductRequestId.From(request.PartnerProductRequestId),
                cancellationToken);

            if(existingQuotation != null)
            {
                return Result.Failure<Guid>(PartnerProductQuotationError.DuplicateRequestId(request.PartnerProductRequestId));
            }

            // lấy partner
            var partner = await _partnerRepository.GetByIdAsync(existingRequest.PartnerId, cancellationToken);
            if(partner == null)
            {
                return Result.Failure<Guid>(PartnerError.NotFound(existingRequest.PartnerId.Value));
            }

            // lấy import service config
            var importServiceConfig = await _importServiceConfigRepository.GetByIdAsync(partner.ImportServiceConfigId, cancellationToken);
            if(importServiceConfig == null)
            {
                return Result.Failure<Guid>(PartnerError.NotFoundImportServiceConfig());
            }

            // tính toán các khoản phí
            // Build a dictionary for quick lookup of custom prices
            var customPriceDict = request.Items?.ToDictionary(
                i => i.PartnerProductId,
                i => i.CustomUnitPrice) ?? new Dictionary<Guid, decimal?>();

            decimal subTotalAmount = 0;
            foreach (var detail in existingRequest.Details)
            {
                decimal unitPrice;
                if (customPriceDict.TryGetValue(detail.PartnerProductId.Value, out var customPrice))
                {
                    unitPrice = customPrice ?? detail.ReferenceUnitPrice;
                }
                else
                {
                    unitPrice = detail.ReferenceUnitPrice;
                }
                subTotalAmount += detail.Quantity * unitPrice;
            }

            decimal importTaxAmount = subTotalAmount * importServiceConfig.ImportTaxPercentage;

            // tạo quotation
            var quotationResult = PartnerProductQuotation.Create(
                code,
                PartnerProductRequestId.From(request.PartnerProductRequestId),
                1,
                subTotalAmount,
                importServiceConfig.BaseShippingFee,
                importTaxAmount,
                request.ExpectedDeliveryDate,
                createdAt: DateTime.UtcNow);

            if (quotationResult.IsFailure)
            {
                return Result.Failure<Guid>(quotationResult.Error);
            }

            var quotation = quotationResult.Value;

            // Create quotation details from request details
            if (existingRequest.Details != null && existingRequest.Details.Any())
            {
                foreach (var item in existingRequest.Details)
                {
                    // Use custom price if provided, otherwise use ReferenceUnitPrice
                    decimal unitPrice;
                    if (customPriceDict.TryGetValue(item.PartnerProductId.Value, out var customPrice))
                    {
                        unitPrice = customPrice ?? item.ReferenceUnitPrice;
                    }
                    else
                    {
                        unitPrice = item.ReferenceUnitPrice;
                    }

                    var detailResult = PartnerProductQuotationDetail.Create(
                        quotation.Id,
                        PartnerProductId.From(item.PartnerProductId.Value),
                        item.Quantity,
                        unitPrice);

                    if (detailResult.IsFailure)
                    {
                        return Result.Failure<Guid>(detailResult.Error);
                    }

                    quotation.AddDetail(detailResult.Value);
                }
            }

            _quotationRepository.Add(quotation);

            // Save all quotation details
            if (quotation.Details.Any())
            {
                foreach (var detail in quotation.Details)
                {
                    _detailRepository.Add(detail);
                }
            }

            return Result.Success(quotation.Id.Value);
        }, cancellationToken);

        return result;
    }
}
