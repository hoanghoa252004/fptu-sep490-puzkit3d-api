using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.UpdatePartnerProductOrderStatus;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductOrders.UpdatePartnerProductOrderStatus;

internal sealed class UpdatePartnerProductOrderStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductOrdersGroup()
            .MapPut("/{id}/status", async (
                [FromRoute] Guid id,
                [FromBody] UpdatePartnerProductOrderStatusRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerProductOrderStatusCommand(id, request.NewStatus);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(success => Results.Ok(new { orderId = success }));
            })
            .WithName("UpdatePartnerProductOrderStatus")
            .WithSummary("Update status of a partner product order")
            .RequireAuthorization()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public sealed record UpdatePartnerProductOrderStatusRequestDto(string NewStatus);
}
