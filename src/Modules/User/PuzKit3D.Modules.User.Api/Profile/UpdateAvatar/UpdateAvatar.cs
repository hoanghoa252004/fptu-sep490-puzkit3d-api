using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateAvatar;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.User.Api.Profile.UpdateAvatar;

internal sealed class UpdateAvatar : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProfileGroup()
            .MapPatch("/avatar", async (
                [FromBody] UpdateAvatarRequestDto request,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateAvatarCommand(
                    currentUser.UserId!,
                    request.AvatarUrl);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateAvatar")
            .WithSummary("Update user avatar")
            .WithDescription("Updates the avatar URL of the currently authenticated user")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateAvatarRequestDto(string AvatarUrl);
