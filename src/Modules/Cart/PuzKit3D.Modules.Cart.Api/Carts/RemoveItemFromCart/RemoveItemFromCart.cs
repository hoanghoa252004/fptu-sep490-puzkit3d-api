using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveItemFromCart;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapDelete("/items", async ([FromBody] RemoveItemFromCartRequestDto request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RemoveItemFromCartCommand(
                    request.UserId,
                    request.CartTypeId,
                    request.ItemId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("RemoveItemFromCart")
            .WithSummary("Remove item from cart")
            .WithDescription("Removes an item from the user's cart")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record RemoveItemFromCartRequestDto(
    Guid UserId,
    Guid CartTypeId,
    Guid ItemId);
