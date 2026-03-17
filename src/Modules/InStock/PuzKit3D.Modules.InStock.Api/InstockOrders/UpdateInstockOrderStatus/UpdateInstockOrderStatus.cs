using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.UpdateInstockOrderStatus;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.UpdateInstockOrderStatus;

internal sealed class UpdateInstockOrderStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapPut("/{id:guid}/status", async (
                Guid id,
                [FromBody] UpdateInstockOrderStatusRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInstockOrderStatusCommand(id, request.NewStatus);
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateInstockOrderStatus")
            .WithSummary("Update instock order status (Staff/Admin only)")
            .WithDescription("Updates the status of an instock order. Validates status transitions based on workflow rules. Requires Staff or BusinessManager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

public sealed record UpdateInstockOrderStatusRequestDto(
    InstockOrderStatus NewStatus);

