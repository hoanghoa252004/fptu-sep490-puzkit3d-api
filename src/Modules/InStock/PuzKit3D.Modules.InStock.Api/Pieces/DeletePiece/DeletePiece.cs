using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.DeletePiece;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.Pieces.DeletePiece;

internal sealed class DeletePiece : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPiecesGroup()
            .MapDelete("/{pieceId:guid}", async (
                Guid productId,
                Guid partId,
                Guid pieceId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeletePieceCommand(productId, partId, pieceId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeletePiece")
            .WithSummary("Delete a piece (Staff/Manager only)")
            .WithDescription("Deletes a specific piece. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
