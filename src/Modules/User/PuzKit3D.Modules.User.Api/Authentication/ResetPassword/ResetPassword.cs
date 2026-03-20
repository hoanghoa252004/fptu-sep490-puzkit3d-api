using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.ResetPassword;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Api.Authentication.ResetPassword;

internal sealed class ResetPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/reset-password", async ([FromBody] ResetPasswordRequestDto request, ISender sender) =>
            {
                var command = new ResetPasswordCommand(
                    request.UserId,
                    request.Token,
                    request.NewPassword);

                Result result = await sender.Send(command);

                return result.MatchOk();
            })
            .WithName("ResetPassword")
            .WithDescription("Reset user password with token")
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record ResetPasswordRequestDto(
    string UserId,
    string Token,
    string NewPassword
);
