using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ChangeUserRole;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.ChangeUserRole;

internal sealed class ChangeUserRole : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{id}/role", async (
                string id,
                [FromBody] ChangeUserRoleRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ChangeUserRoleCommand(id, request.NewRole);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("ChangeUserRole")
            .WithSummary("Change user role (Admin/Manager only)")
            .WithDescription("Changes the role of a user account")
            .RequireAuthorization(Permissions.Users.ChangeRole)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record ChangeUserRoleRequestDto(string NewRole);
