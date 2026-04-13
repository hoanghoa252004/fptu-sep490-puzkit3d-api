using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Commands.CreateDrive;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Drives.CreateDrive;

internal sealed class CreateDrive : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDrivesGroup()
            .MapPost("/", async (
                [FromBody] CreateDriveRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateDriveCommand(
                    request.Name,
                    request.Description,
                    request.MinVolume,
                    request.QuantityInStock,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetDriveById", id => new { id });
            })
            .WithName("CreateDrive")
            .WithSummary("Create a new drive (Staff/Manager only)")
            .WithDescription("Creates a new drive with name, description, min volume, quantity in stock, and active status.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateDriveRequestDto(
string Name,
string? Description,
int? MinVolume,
int QuantityInStock,
bool IsActive);
