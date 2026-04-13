using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.CreateCapability;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.CreateCapability;

internal sealed class CreateCapability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapPost("/", async (
                [FromBody] CreateCapabilityRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCapabilityCommand(
                    request.Name,
                    request.Slug,
                    request.FactorPercentage,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetCapabilityById", id => new { id });
            })
            .WithName("CreateCapability")
            .WithSummary("Create a new capability (Staff/Manager only)")
            .WithDescription("Creates a new capability with name, slug, description, and active status. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCapabilityRequestDto(
string Name,
string Slug,
decimal FactorPercentage,
string? Description,
bool IsActive);
