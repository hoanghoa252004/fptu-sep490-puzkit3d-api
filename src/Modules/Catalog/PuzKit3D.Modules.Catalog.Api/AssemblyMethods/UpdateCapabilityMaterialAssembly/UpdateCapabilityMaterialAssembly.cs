using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.UpdateCapabilityMaterialAssembly;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.UpdateCapabilityMaterialAssembly;

internal sealed class UpdateCapabilityMaterialAssembly : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapPut("/{assemblyId:guid}/capability-material-assemblies/{cmcId:guid}", async (
                Guid assemblyId,
                Guid cmcId,
                [FromBody] UpdateCapabilityMaterialAssemblyRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCapabilityMaterialAssemblyCommand(
                    assemblyId,
                    cmcId,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateCapabilityMaterialAssembly")
            .WithSummary("Update a capability material assembly (Staff/Manager only)")
            .WithDescription("Updates an existing capability material assembly's active status. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateCapabilityMaterialAssemblyRequestDto(
    bool IsActive);
