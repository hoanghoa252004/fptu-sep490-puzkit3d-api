using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Cart.Api.Carts.GetUserCart;

internal sealed class GetUserCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapGet("/{cartType}", async (
                [FromRoute] string cartType, 
                ISender sender, 
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                // Get userId from JWT
                if (!Guid.TryParse(currentUser.UserId, out Guid userId))
                {
                    return Results.BadRequest(new { error = "Invalid user ID" });
                }

                var query = new GetUserCartQuery(userId, cartType);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(cart => Results.Ok(cart));
            })
            .WithName("GetUserCart")
            .WithSummary("Get user's cart by type")
            .WithDescription("Retrieves the customer's cart (INSTOCK or PARTNER) with all items. User must be authenticated.")
            .Produces<CartDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
