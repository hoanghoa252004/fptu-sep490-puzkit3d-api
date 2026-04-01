using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.UpdatePartnerProductRequestStatus;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.UpdatePartnerProductRequestStatus;

internal sealed class UpdatePartnerProductRequestStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductRequestsGroup()
            .MapPut("/{id}/status", async (
                [FromRoute] Guid id,
                [FromBody] UpdatePartnerProductRequestStatusRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerProductRequestStatusCommand(
                    id,
                    request.NewStatus,
                    request.Note);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(success => Results.Ok(new { requestId = success }));
            })
            .WithName("UpdatePartnerProductRequestStatus")
            .WithSummary("Update status of a partner product request (Staff only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public sealed record UpdatePartnerProductRequestStatusRequestDto(
        PartnerProductRequestStatus NewStatus,
        string? Note = null);
}
