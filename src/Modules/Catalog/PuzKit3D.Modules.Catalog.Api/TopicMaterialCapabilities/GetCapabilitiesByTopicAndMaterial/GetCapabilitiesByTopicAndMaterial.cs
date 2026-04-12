using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetCapabilitiesByTopicAndMaterial;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.TopicMaterialCapabilities.GetCapabilitiesByTopicAndMaterial;

internal sealed class GetCapabilitiesByTopicAndMaterial : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFilterGroup()
            .MapGet("/filter-capability", async (
                Guid topicId,
                Guid materialId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCapabilitiesByTopicAndMaterialQuery(topicId, materialId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetCapabilitiesByTopicAndMaterial")
            .WithSummary("Get active capabilities for a topic and material")
            .WithDescription("Retrieves a list of active capabilities that belong to the selected topic and material combination. Returns id, name, and slug for filtering purposes.")
            .Produces<List<GetCapabilityBasicResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
