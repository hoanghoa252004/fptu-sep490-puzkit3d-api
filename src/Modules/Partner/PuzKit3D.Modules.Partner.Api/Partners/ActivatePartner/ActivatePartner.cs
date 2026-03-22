using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.ActivatePartner;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.Partners.ActivatePartner;

internal sealed class ActivatePartner : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapPatch("/{id:guid}/activate", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivatePartnerCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("ActivatePartner")
            .WithSummary("Activate a partner (Staff/Manager only)")
            .WithDescription("Activates a partner by setting IsActive to true. Returns 400 if partner is already active. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
