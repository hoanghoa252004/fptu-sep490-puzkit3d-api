using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetCartItem;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Cart.Api.Carts.GetCartItem;

internal sealed class GetCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapGet("/{cartType}/items/{itemId:guid}", async (
                [FromRoute] string cartType,
                [FromRoute] Guid itemId,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                // Get userId from JWT
                if (!Guid.TryParse(currentUser.UserId, out Guid userId))
                {
                    return Results.BadRequest(new { error = "Invalid user ID" });
                }

                var query = new GetCartItemQuery(userId, cartType, itemId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(item => Results.Ok(item));
            })
            .WithName("GetCartItem")
            .WithSummary("Get specific cart item")
            .WithDescription("Retrieves a specific item from the customer's cart by itemId. User must be authenticated.")
            .Produces<CartItemDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
