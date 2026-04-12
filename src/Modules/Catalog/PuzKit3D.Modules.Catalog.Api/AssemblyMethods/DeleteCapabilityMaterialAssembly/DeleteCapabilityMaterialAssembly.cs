using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityMaterialAssemblies.Commands.DeleteCapabilityMaterialAssembly;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.DeleteCapabilityMaterialAssembly;

internal sealed class DeleteCapabilityMaterialAssembly : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapDelete("/{assemblyId:guid}/capability-material-assemblies/{cmcId:guid}", async (
                Guid assemblyId,
                Guid cmcId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteCapabilityMaterialAssemblyCommand(assemblyId, cmcId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeleteCapabilityMaterialAssembly")
            .WithSummary("Delete a capability material assembly (Staff/Manager only)")
            .WithDescription("Deletes an existing capability material assembly. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
