using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Commands.UpdateUser;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.UpdateUser;

internal sealed class UpdateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapPatch("/{id}", async (
                string id,
                [FromBody] UpdateUserRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateUserCommand(
                    id,
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.ProvinceId,
                    request.ProvinceName,
                    request.DistrictId,
                    request.DistrictName,
                    request.WardCode,
                    request.WardName,
                    request.StreetAddress);
                
                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateUser")
            .WithSummary("Update user information (Admin/Manager only)")
            .WithDescription("Updates user profile information")
            .RequireAuthorization(Permissions.Users.UpdateUser)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateUserRequestDto(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ProvinceId,
    string? ProvinceName,
    string? DistrictId,
    string? DistrictName,
    string? WardCode,
    string? WardName,
    string? StreetAddress);
