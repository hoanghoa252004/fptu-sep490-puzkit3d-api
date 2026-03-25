using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands.UpdateDeliveryTrackingHandOverImage;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.UpdateDeliveryTrackingHandOverImage;

internal sealed class UpdateDeliveryTrackingHandOverImage : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapPut("/{deliveryTrackingId}/hand-over-image", async (
                [FromRoute] Guid deliveryTrackingId,
                [FromBody] UpdateHandOverImageRequest request,
                ISender sender,
                CancellationToken cancellationToken = default) =>
            {
                var command = new UpdateDeliveryTrackingHandOverImageCommand(
                    deliveryTrackingId,
                    request.ImageUrl);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateDeliveryTrackingHandOverImage")
            .WithDescription("Update hand over image URL for a delivery tracking")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}

public sealed record UpdateHandOverImageRequest(string? ImageUrl);
