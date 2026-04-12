using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.UpdateFormula;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.UpdateFormula;

internal sealed class UpdateFormula : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapPut("/{id}", async (
                Guid id,
                [FromBody] UpdateFormulaRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateFormulaCommand(
                    id,
                    request.Expression,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateFormula")
            .WithSummary("Update a formula (Staff/Manager only)")
            .WithDescription("Updates an existing formula. All fields are optional. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateFormulaRequestDto(
string? Expression = null,
string? Description = null);




