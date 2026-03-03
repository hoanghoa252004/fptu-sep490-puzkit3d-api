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
                    request.UserId,
                    request.CartTypeId,
                    request.ItemId,
                    request.UnitPrice,
                    request.InStockProductPriceDetailId,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("AddItemToCart")
            .WithSummary("Add item to cart")
            .WithDescription("Adds an item to the user's cart or increments quantity if item already exists")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record AddItemToCartRequestDto(
    Guid UserId,
    Guid CartTypeId,
    Guid ItemId,
    decimal? UnitPrice,
    Guid? InStockProductPriceDetailId,
    int Quantity);
