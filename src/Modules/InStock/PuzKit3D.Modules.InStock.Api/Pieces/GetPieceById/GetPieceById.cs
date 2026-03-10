using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPieceById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.Pieces.GetPieceById;

internal sealed class GetPieceById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPiecesGroup()
            .MapGet("/{pieceId:guid}", async (
                Guid productId,
                Guid partId,
                Guid pieceId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPieceByIdQuery(productId, partId, pieceId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPieceById")
            .WithSummary("Get piece by ID")
            .WithDescription("Retrieves a specific piece information.")
            .AllowAnonymous()
            .Produces<GetPieceByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
