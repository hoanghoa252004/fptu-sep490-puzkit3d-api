using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.CreatePiece;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.Pieces.CreatePiece;

internal sealed class CreatePiece : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPiecesGroup()
            .MapPost("/", async (
                Guid productId,
                Guid partId,
                [FromBody] CreatePieceRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreatePieceCommand(
                    productId,
                    partId,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock/products/{productId}/parts/{partId}/pieces/{id}", id));
            })
            .WithName("CreatePiece")
            .WithSummary("Create a new piece for a part (Staff/Manager only)")
            .WithDescription("Creates a new piece for the specified part. Code is auto-generated (PIExxxxx). Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreatePieceRequestDto(
    int Quantity);

