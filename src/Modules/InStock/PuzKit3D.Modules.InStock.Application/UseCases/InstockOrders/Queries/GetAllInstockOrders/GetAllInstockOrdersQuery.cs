using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetAllInstockOrders;

public sealed record GetAllInstockOrdersQuery(
    int PageNumber,
    int PageSize,
    InstockOrderStatus? Status) : IQuery<PagedResult<GetAllInstockOrdersResponseDto>>;
