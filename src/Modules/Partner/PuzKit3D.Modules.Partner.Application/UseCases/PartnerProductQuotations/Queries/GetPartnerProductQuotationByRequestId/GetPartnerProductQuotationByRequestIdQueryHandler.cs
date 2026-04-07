using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationByRequestId;

internal sealed class GetPartnerProductQuotationByRequestIdQueryHandler
    : IQueryHandler<GetPartnerProductQuotationByRequestIdQuery, object>
{
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerProductQuotationDetailRepository _detailRepository;

    public GetPartnerProductQuotationByRequestIdQueryHandler(
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerProductQuotationDetailRepository detailRepository)
    {
        _quotationRepository = quotationRepository;
        _detailRepository = detailRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductQuotationByRequestIdQuery request,
        CancellationToken cancellationToken)
    {
        var quotation = await _quotationRepository.GetByRequestIdAsync(
            PartnerProductRequestId.From(request.RequestId),
            cancellationToken);

        if (quotation is null)
        {
            return Result.Failure<object>(
                PartnerProductQuotationError.NotFound(request.RequestId));
        }

        var details = await _detailRepository.FindByQuotationIdAsync(
            quotation.Id,
            cancellationToken);

        var detailDtos = details
            .Select(d => new QuotationDetailItemByIdDto(
                d.Id.Value,
                d.PartnerProductId.Value,
                d.Quantity,
                d.UnitPrice,
                d.TotalAmount))
            .ToList();

        var dto = new GetPartnerProductQuotationByRequestIdResponseDto(
            quotation.Id.Value,
            quotation.Code,
            quotation.PartnerProductRequestId.Value,
            quotation.SubTotalAmount,
            quotation.ShippingFee,
            quotation.ImportTaxAmount,
            quotation.GrandTotalAmount,
            quotation.Note,
            quotation.Status,
            quotation.CreatedAt,
            quotation.UpdatedAt,
            detailDtos);

        return Result.Success<object>(dto);
    }
}
