using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackingBySupportTicketId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetDeliveryTrackingBySupportTicketId;

internal sealed class GetDeliveryTrackingBySupportTicketId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapGet("/support-ticket/{supportTicketId}", async (
                [FromRoute] Guid supportTicketId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetDeliveryTrackingBySupportTicketIdQuery(supportTicketId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetDeliveryTrackingBySupportTicketId")
            .Produces<DeliveryTrackingDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
