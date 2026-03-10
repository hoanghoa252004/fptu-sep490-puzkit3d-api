using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.UpdatePiece;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.Pieces.UpdatePiece;

internal sealed class UpdatePiece : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPiecesGroup()
            .MapPut("/{pieceId:guid}", async (
                Guid productId,
                Guid partId,
                Guid pieceId,
                [FromBody] UpdatePieceRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePieceCommand(
                    productId,
                    partId,
                    pieceId,
                    request.Quantity,
                    request.NewPartId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdatePiece")
            .WithSummary("Update a piece (Staff/Manager only)")
            .WithDescription("Updates quantity and can move piece to another part. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdatePieceRequestDto(
    int Quantity,
    Guid? NewPartId);
