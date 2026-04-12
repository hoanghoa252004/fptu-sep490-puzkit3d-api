using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetAllFormulas;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.GetAllFormulas;

internal sealed class GetAllFormulas : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool ascending,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllFormulasQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    ascending);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllFormulas")
            .WithSummary("Get all formulas with pagination")
            .WithDescription("Retrieves a paginated list of all formulas with optional search filtering.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
