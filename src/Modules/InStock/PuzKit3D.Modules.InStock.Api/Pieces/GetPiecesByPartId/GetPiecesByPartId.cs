using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPiecesByPartId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.Pieces.GetPiecesByPartId;

internal sealed class GetPiecesByPartId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPiecesGroup()
            .MapGet("/", async (
                Guid productId,
                Guid partId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPiecesByPartIdQuery(productId, partId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPiecesByPartId")
            .WithSummary("Get all pieces for a part")
            .WithDescription("Retrieves all pieces belonging to the specified part.")
            .AllowAnonymous()
            .Produces<IReadOnlyList<GetPiecesByPartIdResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
