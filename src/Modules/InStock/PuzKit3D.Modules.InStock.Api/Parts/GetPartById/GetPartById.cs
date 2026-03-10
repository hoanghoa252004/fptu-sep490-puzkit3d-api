using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.Parts.GetPartById;

internal sealed class GetPartById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartsGroup()
            .MapGet("/{partId:guid}", async (
                Guid productId,
                Guid partId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartByIdQuery(productId, partId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartById")
            .WithSummary("Get part by ID with all pieces")
            .WithDescription("Retrieves a specific part with all its pieces.")
            .AllowAnonymous()
            .Produces<GetPartByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
