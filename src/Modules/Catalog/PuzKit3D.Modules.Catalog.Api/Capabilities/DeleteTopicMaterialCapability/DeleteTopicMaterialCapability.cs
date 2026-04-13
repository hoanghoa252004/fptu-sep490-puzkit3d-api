using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.DeleteTopicMaterialCapability;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.DeleteTopicMaterialCapability;

internal sealed class DeleteTopicMaterialCapability : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapDelete("/{capabilityId:guid}/topic-material-capabilities/{tmcId:guid}", async (
                Guid capabilityId,
                Guid tmcId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteTopicMaterialCapabilityCommand(capabilityId, tmcId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeleteTopicMaterialCapability")
            .WithSummary("Delete a topic material capability (Staff/Manager only)")
            .WithDescription("Deletes an existing topic material capability. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
