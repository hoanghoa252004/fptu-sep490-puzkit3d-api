using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetAllPartnerProductQuotations;

internal sealed class GetAllPartnerProductQuotationsQueryHandler 
    : IQueryHandler<GetAllPartnerProductQuotationsQuery, PagedResult<GetAllPartnerProductQuotationsResponseDto>>
{
    private readonly IPartnerProductQuotationRepository _repository;

    public GetAllPartnerProductQuotationsQueryHandler(IPartnerProductQuotationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<GetAllPartnerProductQuotationsResponseDto>>> Handle(
        GetAllPartnerProductQuotationsQuery request,
        CancellationToken cancellationToken)
    {
        var quotations = await _repository.GetAllAsync(
            request.CreatedAtFrom,
            request.CreatedAtTo,
            request.Ascending,
            cancellationToken);

        var totalCount = quotations.Count();
        var dtos = quotations
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(q => new GetAllPartnerProductQuotationsResponseDto(
                q.Id.Value,
                q.Code,
                q.PartnerProductRequestId.Value,
                q.SubTotalAmount,
                q.ShippingFee,
                q.ImportTaxAmount,
                q.GrandTotalAmount,
                q.ExpectedDeliveryDate,
                q.Note,
                q.Status,
                q.CreatedAt,
                q.UpdatedAt))
            .ToList();

        var pagedResult = new PagedResult<GetAllPartnerProductQuotationsResponseDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
