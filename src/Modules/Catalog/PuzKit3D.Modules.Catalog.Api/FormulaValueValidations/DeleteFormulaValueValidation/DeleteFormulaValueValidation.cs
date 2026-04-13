using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.DeleteFormulaValueValidation;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.FormulaValueValidations.DeleteFormulaValueValidation;

internal sealed class DeleteFormulaValueValidation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulaValueValidationsGroup()
            .MapDelete("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteFormulaValueValidationCommand(id);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchNoContent();
            })
            .WithName("DeleteFormulaValueValidation")
            .WithSummary("Delete a formula value validation (Staff/Manager only)")
            .WithDescription("Deletes an existing formula value validation. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
