using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCartItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Cart.Api.PartnerCarts.GetCartItem;

internal sealed class GetPartnerCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerCartsGroup()
            .MapGet("/items/{itemId:guid}", async (
                [FromRoute] Guid itemId,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                if (!Guid.TryParse(currentUser.UserId, out Guid userId))
                {
                    return Results.BadRequest(new { error = "Invalid user ID" });
                }

                var query = new GetPartnerCartItemQuery(userId, itemId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(item => Results.Ok(item));
            })
            .WithName("GetPartnerCartItem")
            .WithSummary("Get specific Partner cart item")
            .WithDescription("Retrieves a specific item from the customer's Partner cart. User must be authenticated.")
            .Produces<CartItemDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
