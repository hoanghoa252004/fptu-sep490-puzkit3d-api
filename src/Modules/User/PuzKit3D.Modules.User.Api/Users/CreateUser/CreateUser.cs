using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.CreateStaffOrManager;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.CreateUser;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.CreateUser;

internal sealed class CreateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPost("/staff-or-manager", async (
                [FromBody] CreateUserRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateUserCommand(
                    request.Email,
                    request.Password,
                    request.Role,
                    request.FirstName,
                    request.LastName);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetUserById", message => new { message });
            })
            .WithName("CreateUser")
            .WithSummary("Create Staff or Manager account (Admin only)")
            .WithDescription("Creates a new user account with Staff or Manager role. Only Admin can access this endpoint.")
            .RequireAuthorization(Permissions.Users.CreateUser)
            .Produces<string>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateUserRequestDto(
    string Email,
    string Password,
    string Role,
    string? FirstName,
    string? LastName);
