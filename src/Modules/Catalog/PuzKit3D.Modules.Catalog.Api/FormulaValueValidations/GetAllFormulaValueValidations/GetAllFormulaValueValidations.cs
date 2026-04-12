using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Queries.GetAllFormulaValueValidations;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.FormulaValueValidations.GetAllFormulaValueValidations;

internal sealed class GetAllFormulaValueValidations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulaValueValidationsGroup()
            .MapGet("/", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllFormulaValueValidationsQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllFormulaValueValidations")
            .WithSummary("Get all formula value validations")
            .WithDescription("Retrieves all formula value validations.")
            .AllowAnonymous()
            .Produces<List<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}


