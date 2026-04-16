using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.CreateCapabilityDrive;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.CreateCapabilityDrive;

internal sealed class CreateCapabilityDrive : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapPost("/{id:guid}/capability-drives", async (
                Guid id,
                [FromBody] CreateCapabilityDriveRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCapabilityDriveCommand(id, request.DriveId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetCapabilityDrivesByCapabilityId", _ => new { id });
            })
            .WithName("CreateCapabilityDrive")
            .WithSummary("Create a new capability drive (Staff/Manager only)")
            .WithDescription("Creates a new capability drive linking a capability and drive. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<object>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCapabilityDriveRequestDto(Guid DriveId);
