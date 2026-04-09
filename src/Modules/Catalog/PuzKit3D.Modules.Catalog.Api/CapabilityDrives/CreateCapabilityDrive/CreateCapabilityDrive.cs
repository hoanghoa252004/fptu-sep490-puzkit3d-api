using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Commands.CreateCapabilityDrive;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.CapabilityDrives.CreateCapabilityDrive;

internal sealed class CreateCapabilityDrive : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilityDrivesGroup()
            .MapPost("/", async (
                [FromBody] CreateCapabilityDriveRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCapabilityDriveCommand(
                    request.CapabilityId,
                    request.DriveId,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetCapabilityDriveById", id => new { id });
            })
            .WithName("CreateCapabilityDrive")
            .WithSummary("Create a capability-drive link (Staff/Manager only)")
            .WithDescription("Creates a link between a capability and a drive.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCapabilityDriveRequestDto(
    Guid CapabilityId,
    Guid DriveId,
    int Quantity);
