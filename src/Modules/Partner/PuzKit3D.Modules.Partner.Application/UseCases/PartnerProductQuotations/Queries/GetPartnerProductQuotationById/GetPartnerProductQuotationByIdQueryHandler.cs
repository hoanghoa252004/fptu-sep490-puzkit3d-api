using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationById;

internal sealed class GetPartnerProductQuotationByIdQueryHandler
    : IQueryHandler<GetPartnerProductQuotationByIdQuery, object>
{
    private readonly IPartnerProductQuotationRepository _quotationRepository;
    private readonly IPartnerProductQuotationDetailRepository _detailRepository;

    public GetPartnerProductQuotationByIdQueryHandler(
        IPartnerProductQuotationRepository quotationRepository,
        IPartnerProductQuotationDetailRepository detailRepository)
    {
        _quotationRepository = quotationRepository;
        _detailRepository = detailRepository;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerProductQuotationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var quotation = await _quotationRepository.GetByIdAsync(
            PartnerProductQuotationId.From(request.Id),
            cancellationToken);

        if (quotation is null)
        {
            return Result.Failure<object>(PartnerProductQuotationError.NotFound(request.Id));
        }

        var details = await _detailRepository.FindByQuotationIdAsync(
            PartnerProductQuotationId.From(request.Id),
            cancellationToken);

        var detailDtos = details
            .Select(d => new QuotationDetailItemDto(
                d.Id.Value,
                d.PartnerProductId.Value,
                d.Quantity,
                d.UnitPrice,
                d.TotalAmount))
            .ToList();

        var dto = new GetPartnerProductQuotationByIdResponseDto(
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
