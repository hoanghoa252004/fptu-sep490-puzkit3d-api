using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.ChangePassword;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.User.Api.Profile.ChangePassword;

internal sealed class ChangePassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProfileGroup()
            .MapPatch("/password", async (
                [FromBody] ChangePasswordRequestDto request,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var command = new ChangePasswordCommand(
                    currentUser.UserId!,
                    request.CurrentPassword,
                    request.NewPassword);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("ChangePassword")
            .WithSummary("Change password")
            .WithDescription("Changes the password of the currently authenticated user")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record ChangePasswordRequestDto(
    string CurrentPassword,
    string NewPassword);
