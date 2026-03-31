using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.Services;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;

internal sealed class CreatePartnerProductRequestCommandHandler : ICommandTHandler<CreatePartnerProductRequestCommand, Guid>
{
    private readonly IPartnerProductRequestRepository _requestRepository;
    private readonly IPartnerProductRequestDetailRepository _detailRepository;
    private readonly IPartnerProductRepository _productRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IPartnerProductRequestCodeGenerator _codeGenerator;

    public CreatePartnerProductRequestCommandHandler(
        IPartnerProductRequestRepository requestRepository,
        IPartnerProductRequestDetailRepository detailRepository,
        IPartnerProductRepository productRepository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductRequestCodeGenerator codeGenerator)
    {
        _requestRepository = requestRepository;
        _detailRepository = detailRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
    }

    public async Task<ResultT<Guid>> Handle(
        CreatePartnerProductRequestCommand request,
        CancellationToken cancellationToken)
    {
        // Validate all items belong to the same partner
        var products = await _productRepository.GetAllAsync(cancellationToken);
        var requestProductIds = request.Items.Select(i => i.PartnerProductId).ToList();

        var productsInRequest = products
            .Where(p => requestProductIds.Contains(p.Id.Value))
            .ToList();

        if (productsInRequest.Count != request.Items.Count)
        {
            return Result.Failure<Guid>(
                PartnerProductRequestError.NotFound(Guid.Empty));
        }

        // Check all products belong to the same partner
        var uniquePartnerIds = productsInRequest.Select(p => p.PartnerId.Value).Distinct().ToList();
        if (uniquePartnerIds.Count != 1 || uniquePartnerIds[0] != request.PartnerId)
        {
            return Result.Failure<Guid>(
                PartnerProductRequestError.InvalidQuantity());
        }
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            var orderCode = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            var requestResult = PartnerProductRequest.Create(
                orderCode,
                request.CustomerId,
                PartnerId.From(request.PartnerId),
                request.DesiredDeliveryDate,
                request.Items.Select(i => (
                    PartnerProductId.From(i.PartnerProductId),
                    i.Quantity,
                    productsInRequest.First(p => p.Id.Value == i.PartnerProductId).ReferencePrice
                )).ToList(),
                (int)PartnerProductRequestStatus.Pending,
                DateTime.UtcNow
            );

            if (requestResult.IsFailure)
            {
                return Result.Failure<Guid>(requestResult.Error);
            }

            _requestRepository.Add(requestResult.Value);

            return Result.Success(requestResult.Value.Id.Value);
        }, cancellationToken);

        return result;
    }
}
