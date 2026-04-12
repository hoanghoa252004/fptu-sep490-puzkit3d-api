using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.GetAssemblyMethodsByCapabilityAndMaterial;

internal sealed class GetAssemblyMethodsByCapabilityAndMaterial : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFilterGroup()
            .MapGet("/filter-assembly-method", async (
                Guid capabilityId,
                Guid materialId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAssemblyMethodsByCapabilityAndMaterialQuery(capabilityId, materialId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAssemblyMethodsByCapabilityAndMaterial")
            .WithSummary("Get active assembly methods for a capability and material")
            .WithDescription("Retrieves a list of active assembly methods that belong to the selected capability and material combination. Returns id, name, and slug for filtering purposes.")
            .Produces<List<GetAssemblyMethodBasicResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
