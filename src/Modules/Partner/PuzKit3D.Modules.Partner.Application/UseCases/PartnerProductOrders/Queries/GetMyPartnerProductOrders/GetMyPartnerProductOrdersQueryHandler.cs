using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetMyPartnerProductOrders;

internal sealed class GetMyPartnerProductOrdersQueryHandler
    : IQueryHandler<GetMyPartnerProductOrdersQuery, PagedResult<object>>
{
    private readonly IPartnerProductOrderRepository _repository;

    public GetMyPartnerProductOrdersQueryHandler(IPartnerProductOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetMyPartnerProductOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByCustomerIdAsync(
            request.CustomerId,
            request.Status,
            cancellationToken);

        var query = orders.AsQueryable();

        var totalCount = query.Count();
        var paginatedOrders = query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paginatedOrders
            .Select(o => (object)new GetMyPartnerProductOrdersResponseDto(
                o.Id.Value,
                o.PartnerProductQuotationId.Value,
                o.Code,
                o.SubTotalAmount,
                o.ShippingFee,
                o.ImportTaxAmount,
                o.GrandTotalAmount,
                o.Status.ToString(),
                o.PaymentMethod,
                o.IsPaid,
                o.CreatedAt,
                o.UpdatedAt))
            .ToList();

        var pagedResult = new PagedResult<object>(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result.Success(pagedResult);
    }
}
