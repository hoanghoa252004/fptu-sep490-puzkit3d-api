using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.UpdateTopicMaterialCapability;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.UpdateTopicMaterialCapability;

internal sealed class UpdateTopicMaterialCapability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapPut("/{capabilityId:guid}/topic-material-capabilities/{tmcId:guid}", async (
                Guid capabilityId,
                Guid tmcId,
                [FromBody] UpdateTopicMaterialCapabilityRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateTopicMaterialCapabilityCommand(
                    capabilityId,
                    tmcId,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateTopicMaterialCapability")
            .WithSummary("Update a topic material capability (Staff/Manager only)")
            .WithDescription("Updates an existing topic material capability's active status. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateTopicMaterialCapabilityRequestDto(
    bool IsActive);
