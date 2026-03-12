using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrderById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetCustomerOrderById;

internal sealed class GetCustomerOrderById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapGet("/{orderId:guid}", async (
                Guid orderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCustomerOrderByIdQuery(orderId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetCustomerOrderById")
            .WithSummary("Get customer order by ID with full details")
            .WithDescription("Retrieves complete order information including all order details with product and variant information. Only the order owner can access.")
            .RequireAuthorization()
            .Produces<GetCustomerOrderByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
