using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.CreateDeliverTracking;

public sealed class CreateDeliveryTracking : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapPost("/", async (
                [FromBody] CreateSupportTicketRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Validate and convert string type to enum
                if (!Enum.TryParse<DeliveryTrackingType>(request.DeliveryTrackingType, ignoreCase: true, out var typeEnum))
                {
                    return Results.BadRequest(new { error = $"Invalid type '{request.DeliveryTrackingType}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(DeliveryTrackingType)))}" });
                }
                var query = new CreateDeliveryTrackingCommand(request.OrderId, typeEnum);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("CreateDeliveryTracking")
            .WithDescription("Create GHN shipping order and get delivery order code by InstockOrder ID")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
public sealed record CreateSupportTicketRequestDto(
    Guid OrderId,
    string DeliveryTrackingType);
