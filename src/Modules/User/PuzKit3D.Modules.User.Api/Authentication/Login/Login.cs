using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.Login;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Api.Authentication.Login;

public sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/login", async (LoginRequest request, ISender sender) =>
            {
                var command = new LoginCommand(request.Email, request.Password);
                
                ResultT<AuthenticationResult> result = await sender.Send(command);

                return result.MatchOk();
            })
            .WithName("Login")
            .WithSummary("Login with email and password")
            .WithDescription("Login with email and password, response jwt token")
            .AllowAnonymous()
            .Produces<AuthenticationResult>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public sealed record LoginRequest(string Email, string Password);
