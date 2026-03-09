using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartsByProductId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.Parts.GetPartsByProductId;

internal sealed class GetPartsByProductId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartsGroup()
            .MapGet("/", async (
                Guid productId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartsByProductIdQuery(productId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartsByProductId")
            .WithSummary("Get all parts for a product")
            .WithDescription("Retrieves all parts belonging to the specified product.")
            .AllowAnonymous()
            .Produces<IReadOnlyList<GetPartsByProductIdResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
