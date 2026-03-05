using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.Logout;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.User.Api.Authentication.Logout;

internal sealed class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/logout", async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var command = new LogoutCommand(currentUser.UserId!);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("Logout")
            .WithSummary("Logout user")
            .WithDescription("Logs out the current user by invalidating the refresh token")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
