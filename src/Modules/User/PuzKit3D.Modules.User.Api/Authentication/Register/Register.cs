using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Api.Authentication.Register;

public sealed class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAuthenticationGroup()
            .MapPost("/register", async ([FromBody] RegisterRequestDto request, ISender sender) =>
            {
                var command = new RegisterCommand(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName);
                
                ResultT<string> result = await sender.Send(command);

                return result.MatchOk();
            })
            .WithName("Register")
            .WithSummary("Register new account")
            .WithDescription("Create a new user account in the system")
            .AllowAnonymous()
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}





