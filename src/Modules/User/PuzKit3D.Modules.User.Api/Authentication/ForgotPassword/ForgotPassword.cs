using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.ForgotPassword;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Api.Authentication.ForgotPassword;

internal sealed class ForgotPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/forgot-password", async ([FromBody] ForgotPasswordRequestDto request, ISender sender) =>
            {
                var command = new ForgotPasswordCommand(request.Email);

                Result result = await sender.Send(command);

                return result.MatchOk();
            })
            .WithName("ForgotPassword")
            .WithDescription("Request a password reset email")
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record ForgotPasswordRequestDto(string Email);
