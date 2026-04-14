using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Queries.GetAssemblyMethodsByCapabilityAndMaterial;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using System.Collections.Generic;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.GetAssemblyMethodsByCapabilityAndMaterial;

public sealed record GetAssemblyMethodsRequest(List<Guid> CapabilityIds, Guid MaterialId);

internal sealed class GetAssemblyMethodsByCapabilityAndMaterial : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFilterGroup()
            .MapGet("/filter-assembly-method", async (
                [FromQuery] Guid MaterialId,
                [FromQuery] Guid[] CapabilityIds,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAssemblyMethodsByCapabilityAndMaterialQuery(CapabilityIds.ToList(), MaterialId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAssemblyMethodsByCapabilityAndMaterial")
            .WithSummary("Get active assembly methods for capabilities and material")
            .WithDescription("Retrieves a list of active assembly methods that belong to the selected capabilities and material combinations. Returns id, name, and slug for filtering purposes.")
            .Produces<List<GetAssemblyMethodBasicResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
