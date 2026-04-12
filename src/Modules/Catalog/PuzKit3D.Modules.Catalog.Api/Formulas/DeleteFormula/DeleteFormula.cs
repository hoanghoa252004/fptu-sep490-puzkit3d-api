using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.DeleteFormula;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.DeleteFormula;

internal sealed class DeleteFormula : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapDelete("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteFormulaCommand(id);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchNoContent();
            })
            .WithName("DeleteFormula")
            .WithSummary("Delete a formula (Staff/Manager only)")
            .WithDescription("Deletes an existing formula. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
