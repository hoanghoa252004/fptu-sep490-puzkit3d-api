using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.DeleteAssemblyMethod;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.DeleteAssemblyMethod;

internal sealed class DeleteAssemblyMethod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapDelete("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteAssemblyMethodCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("DeleteAssemblyMethod")
            .WithSummary("Delete an assembly method (Staff/Manager only)")
            .WithDescription("Deletes an existing assembly method by ID. Requires Staff or Manager permission.")
            .RequireAuthorization(Permissions.Catalog.ManageAssemblyMethods)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}


