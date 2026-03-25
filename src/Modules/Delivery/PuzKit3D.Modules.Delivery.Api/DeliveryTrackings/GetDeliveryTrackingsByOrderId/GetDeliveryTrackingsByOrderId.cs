using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingsByOrderId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetDeliveryTrackingsByOrderId;

internal sealed class GetDeliveryTrackingsByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapGet("/order/{orderId}", async (
                [FromRoute] Guid orderId,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? status = null,
                ISender sender = default!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetDeliveryTrackingsByOrderIdQuery(orderId, pageNumber, pageSize, status);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetDeliveryTrackingsByOrderId")
            .Produces<PaginatedDeliveryTrackingDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}



