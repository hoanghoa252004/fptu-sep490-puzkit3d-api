using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateProfile;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.User.Api.Profile.UpdateProfile;

internal sealed class UpdateProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProfileGroup()
            .MapPatch("/", async (
                [FromBody] UpdateProfileRequestDto request,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateProfileCommand(
                    currentUser.UserId!,
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
            .WithName("UpdateProfile")
            .WithSummary("Update current user profile")
            .WithDescription("Updates the profile information of the currently authenticated user")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateProfileRequestDto(
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
