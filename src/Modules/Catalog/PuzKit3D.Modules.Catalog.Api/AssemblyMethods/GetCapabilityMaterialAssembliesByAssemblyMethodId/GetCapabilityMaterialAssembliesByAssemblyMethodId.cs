using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetCapabilityMaterialAssembliesByAssemblyMethodId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.GetCapabilityMaterialAssembliesByAssemblyMethodId;

internal sealed class GetCapabilityMaterialAssembliesByAssemblyMethodId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapGet("/{id:guid}/capability-material-assemblies", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCapabilityMaterialAssembliesByAssemblyMethodIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetCapabilityMaterialAssembliesByAssemblyMethodId")
            .WithSummary("Get capability material assemblies by assembly method ID")
            .WithDescription("Retrieves all capability material assemblies associated with a specific assembly method. Anonymous users can only view active assembly methods.")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
