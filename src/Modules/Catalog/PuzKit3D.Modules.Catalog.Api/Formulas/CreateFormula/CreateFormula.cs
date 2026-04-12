using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Commands.CreateFormula;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.CreateFormula;

internal sealed class CreateFormula : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapPost("/", async (
                [FromBody] CreateFormulaRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateFormulaCommand(
                    request.Code,
                    request.Expression,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetFormulaById", id => new { id });
            })
            .WithName("CreateFormula")
            .WithSummary("Create a new formula (Staff/Manager only)")
            .WithDescription("Creates a new formula with code, expression, and optional description. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateFormulaRequestDto(
    string Code,
    string Expression,
    string? Description);
