using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveCartItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.Carts.RemoveCartItem;

internal sealed class RemoveCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapDelete("/items", async ([FromQuery] string itemType, [FromQuery] Guid itemId, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new RemoveCartItemCommand(itemType, itemId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("RemoveCartItem")
            .WithSummary("Remove item from cart")
            .WithDescription("Removes an item from the customer's cart. Supports both 'instock' and 'partner' item types. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
