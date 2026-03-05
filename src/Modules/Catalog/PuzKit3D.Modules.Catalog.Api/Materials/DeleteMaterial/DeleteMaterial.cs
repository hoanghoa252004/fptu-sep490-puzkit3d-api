using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.DeleteMaterial;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Materials.DeleteMaterial;

internal sealed class DeleteMaterial : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMaterialsGroup()
            .MapDelete("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteMaterialCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeleteMaterial")
            .WithSummary("Delete a material (Staff/Manager only)")
            .WithDescription("Deletes an existing material by its ID. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
