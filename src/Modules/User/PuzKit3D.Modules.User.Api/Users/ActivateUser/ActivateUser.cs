using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ActivateUser;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.ActivateUser;

internal sealed class ActivateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{id}/activate", async (
                string id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivateUserCommand(id);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("ActivateUser")
            .WithSummary("[Admin]")
            .WithDescription("Restores a soft-deleted user account. Requires Admin role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
