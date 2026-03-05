using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.RefreshToken;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authentication;

namespace PuzKit3D.Modules.User.Api.Authentication.RefreshToken;

internal sealed class RefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/refresh-token", async (
                RefreshTokenRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RefreshTokenCommand(request.RefreshToken);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("RefreshToken")
            .WithDescription("Refreshes the access token using a valid refresh token")
            .AllowAnonymous()
            .Produces<AuthenticationResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized) // Invalid or expired token
            .ProducesProblem(StatusCodes.Status403Forbidden) // Account locked
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record RefreshTokenRequest(string RefreshToken);
