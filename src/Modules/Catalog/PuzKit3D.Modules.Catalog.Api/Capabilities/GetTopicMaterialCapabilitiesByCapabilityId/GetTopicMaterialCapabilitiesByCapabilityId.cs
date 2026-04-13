using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetTopicMaterialCapabilitiesByCapabilityId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.GetTopicMaterialCapabilitiesByCapabilityId;

internal sealed class GetTopicMaterialCapabilitiesByCapabilityId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilitiesGroup()
            .MapGet("/{id:guid}/topic-material-capabilities", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetTopicMaterialCapabilitiesByCapabilityIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetTopicMaterialCapabilitiesByCapabilityId")
            .WithSummary("Get topic material capabilities by capability ID")
            .WithDescription("Retrieves all topic material capabilities associated with a specific capability. Anonymous users can only view active capabilities.")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
