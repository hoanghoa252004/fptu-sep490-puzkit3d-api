using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetAllFormulaValueValidations;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.FormulaValueValidations.GetAllFormulaValueValidations;

internal sealed class GetAllFormulaValueValidations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulaValueValidationsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                Guid? formulaId,
                bool ascending,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllFormulaValueValidationsQuery(
                    pageNumber,
                    pageSize,
                    formulaId,
                    ascending);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllFormulaValueValidations")
            .WithSummary("Get all formula value validations with pagination")
            .WithDescription("Retrieves a paginated list of formula value validations with optional filtering by formula ID.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
