using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CalculateFormula;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.CalculateFormula;

internal sealed class CalculateFormula : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapPost("/{formulaCode:required}/calculate", async (
                string formulaCode,
                [FromBody] FormulaCalculateRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new FormulaCalculateQuery(formulaCode, request);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("CalculateFormula")
            .WithSummary("Calculate formula result")
            .WithDescription("Evaluates a formula with provided variables and returns the calculated result. If formula has validation ranges, returns the matched output value.")
            .AllowAnonymous()
            .Produces<FormulaCalculateResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

