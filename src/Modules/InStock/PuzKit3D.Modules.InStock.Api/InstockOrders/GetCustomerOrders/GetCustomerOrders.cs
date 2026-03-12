using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetCustomerOrders;

internal sealed class GetCustomerOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapGet("/customer", async (
                int pageNumber,
                int pageSize,
                InstockOrderStatus? status,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCustomerOrdersQuery(
                    pageNumber,
                    pageSize,
                    status);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetCustomerOrders")
            .WithSummary("Get customer orders with pagination and filter by status")
            .WithDescription("Retrieves a paginated list of orders for the authenticated customer. Can filter by order status.")
            .RequireAuthorization()
            .Produces<PagedResult<GetCustomerOrdersResponseDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
