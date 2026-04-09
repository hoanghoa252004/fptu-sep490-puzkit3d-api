using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.UpdateDrive;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Drives.UpdateDrive;

internal sealed class UpdateDrive : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDrivesGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdateDriveRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateDriveCommand(
                    id,
                    request.Name,
                    request.Description,
                    request.MinVolume,
                    request.QuantityInStock,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateDrive")
            .WithSummary("Update a drive (Staff/Manager only)")
            .WithDescription("Updates an existing drive with the provided information.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateDriveRequestDto(
    string? Name,
    string? Description,
    int? MinVolume,
    int? QuantityInStock,
    bool? IsActive);
