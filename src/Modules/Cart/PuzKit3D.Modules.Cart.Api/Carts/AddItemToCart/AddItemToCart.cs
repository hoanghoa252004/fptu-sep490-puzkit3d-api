using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.Carts.AddItemToCart;

internal sealed class AddItemToCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCartsGroup()
            .MapPost("/items", async ([FromBody] AddItemToCartRequestDto request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new AddItemToCartCommand(
                    request.ItemType,
                    request.ItemId,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("AddItemToCart")
            .WithSummary("Add item to cart")
            .WithDescription("Adds an item to the customer's cart. Supports both 'instock' and 'partner' item types. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}

internal sealed record AddItemToCartRequestDto(
    string ItemType,
    Guid ItemId,
    int? Quantity
);

