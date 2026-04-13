using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.CreateCapabilityMaterialAssembly;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.CreateCapabilityMaterialAssembly;

internal sealed class CreateCapabilityMaterialAssembly : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapPost("/{id:guid}/capability-material-assemblies", async (
                Guid id,
                [FromBody] CreateCapabilityMaterialAssemblyRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCapabilityMaterialAssemblyCommand(
                    id,
                    request.CapabilityId,
                    request.MaterialId,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetCapabilityMaterialAssembliesByAssemblyMethodId", _ => new { id });
            })
            .WithName("CreateCapabilityMaterialAssembly")
            .WithSummary("Create a capability material assembly for an assembly method (Staff/Manager only)")
            .WithDescription("Creates a new capability material assembly linking a capability and material to a specific assembly method. The assembly method ID is provided in the URL path. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCapabilityMaterialAssemblyRequestDto(
    Guid CapabilityId,
    Guid MaterialId,
    bool IsActive = false);
