using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.UpdateCartItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.Carts.UpdateCartItem;

internal sealed class UpdateCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapPut("/items", async ([FromBody] UpdateCartItemRequestDto request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateCartItemCommand(
                    request.ItemType,
                    request.ItemId,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateCartItem")
            .WithSummary("Update cart item quantity")
            .WithDescription("Updates the quantity of an item in the customer's cart. Supports both 'instock' and 'partner' item types. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}

internal sealed record UpdateCartItemRequestDto(
    string ItemType,
    Guid ItemId,
    int Quantity
);
