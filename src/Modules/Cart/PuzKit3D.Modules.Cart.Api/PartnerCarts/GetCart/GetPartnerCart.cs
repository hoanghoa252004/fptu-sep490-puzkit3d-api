using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCart;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Cart.Api.PartnerCarts.GetCart;

internal sealed class GetPartnerCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerCartsGroup()
            .MapGet("/items", async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                if (!Guid.TryParse(currentUser.UserId, out Guid userId))
                {
                    return Results.BadRequest(new { error = "Invalid user ID" });
                }

                var query = new GetPartnerCartQuery(userId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(cart => Results.Ok(cart));
            })
            .WithName("GetPartnerCart")
            .WithSummary("Get Partner cart")
            .WithDescription("Retrieves the customer's Partner cart with all items. User must be authenticated.")
            .Produces<CartDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
