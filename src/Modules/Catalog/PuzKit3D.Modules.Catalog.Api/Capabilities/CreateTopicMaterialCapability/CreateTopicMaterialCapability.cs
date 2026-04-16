using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.CreateTopicMaterialCapability;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.CreateTopicMaterialCapability;

internal sealed class CreateTopicMaterialCapability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapPost("/{id:guid}/topic-material-capabilities", async (
                Guid id,
                [FromBody] CreateTopicMaterialCapabilityRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateTopicMaterialCapabilityCommand(
                    id,
                    request.TopicId,
                    request.MaterialId,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetTopicMaterialCapabilitiesByCapabilityId", _ => new { id });
            })
            .WithName("CreateTopicMaterialCapability")
            .WithSummary("Create a topic material capability for a capability (Staff/Manager only)")
            .WithDescription("Creates a new topic material capability linking a topic and material to a specific capability. The capability ID is provided in the URL path. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateTopicMaterialCapabilityRequestDto(
    Guid TopicId,
    Guid MaterialId,
    bool IsActive = false);
