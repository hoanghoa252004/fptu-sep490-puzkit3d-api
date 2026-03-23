using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetDeliveryTrackingsByOrderId;

internal sealed class GetDeliveryTrackingsByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/delivery-trackings")
            .WithTags("Delivery Trackings")
            .MapGet("/order/{orderId}", HandleGetByOrderId)
            .WithName("GetDeliveryTrackingsByOrderId")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleGetByOrderId(
        [FromRoute] Guid orderId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        ISender sender = default!,
        CancellationToken cancellationToken = default)
    {
        var query = new GetDeliveryTrackingsByOrderIdQuery(orderId, pageNumber, pageSize, status);
        var result = await sender.Send(query, cancellationToken);

        return result.MatchOk();
    }
}

