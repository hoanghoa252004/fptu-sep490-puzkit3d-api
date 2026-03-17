using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetGhnOrderCodeByInstockOrderId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetGhnOrderCode;

internal sealed class GetGhnOrderCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapPost("/{orderId}/ghn-order-code", async (
                [FromRoute] Guid orderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetGhnOrderCodeQuery(orderId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetGhnOrderCode")
            .WithDescription("Get GHN shipping order code by InstockOrder ID")
            .Produces<GetGhnOrderCodeResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
