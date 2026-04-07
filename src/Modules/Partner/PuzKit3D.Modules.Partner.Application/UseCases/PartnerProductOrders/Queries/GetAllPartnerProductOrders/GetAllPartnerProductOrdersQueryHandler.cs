using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetAllPartnerProductOrders;

internal sealed class GetAllPartnerProductOrdersQueryHandler
    : IQueryHandler<GetAllPartnerProductOrdersQuery, PagedResult<object>>
{
    private readonly IPartnerProductOrderRepository _repository;

    public GetAllPartnerProductOrdersQueryHandler(IPartnerProductOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllPartnerProductOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync(
            request.Status,
            request.CreatedAtFrom,
            request.CreatedAtTo,
            request.Ascending,
            cancellationToken);

        var query = orders.AsQueryable();

        var totalCount = query.Count();
        var paginatedOrders = query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = paginatedOrders
            .Select(o => (object)new GetAllPartnerProductOrdersResponseDto(
                o.Id.Value,
                o.PartnerProductQuotationId.Value,
                o.Code,
                o.CustomerId,
                o.CustomerName,
                o.CustomerPhone,
                o.CustomerEmail,
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
