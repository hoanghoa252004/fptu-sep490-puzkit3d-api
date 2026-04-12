using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.GetFormulaById;
using PuzKit3D.Modules.Catalog.Application.UseCases.Formulas.Queries.Shared;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Formulas.GetFormulaById;

internal sealed class GetFormulaById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulasGroup()
            .MapGet("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetFormulaByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetFormulaById")
            .WithSummary("Get a formula by ID")
            .WithDescription("Retrieves a specific formula by its ID.")
            .AllowAnonymous()
            .Produces<GetFormulaDetailedResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
