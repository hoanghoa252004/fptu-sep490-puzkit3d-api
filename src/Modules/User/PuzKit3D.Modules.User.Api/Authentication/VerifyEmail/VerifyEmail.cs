using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Api.Authentication.Register;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.VerifyEmail;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.User.Api.Authentication.VerifyEmail;

internal sealed class VerifyEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/confirm-email", async ([FromBody] VerifyEmailRequestDto request, ISender sender) =>
            {
                var command = new VerifyEmailCommand(
                    request.UserId,
                    request.Token);

                Result result = await sender.Send(command);

                return result.MatchOk();
            })
            .WithName("ConfirmEmail")
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
internal sealed record VerifyEmailRequestDto(
    string UserId,
    string Token
);

