using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.UnlockUser;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.UnlockUser;

internal sealed class UnlockUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{id}/unlock", async (
                string id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UnlockUserCommand(id);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UnlockUser")
            .WithSummary("Unlock user account (Admin/Manager only)")
            .WithDescription("Unlocks a user account allowing them to log in again")
            .RequireAuthorization(Permissions.Users.UnlockUser)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
