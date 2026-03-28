using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.UpdateItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Cart.Api.InStockCarts.UpdateItem;

internal sealed class UpdateInStockCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapInStockCartsGroup()
            .MapPut("/items/{itemId:guid}", async (
                [FromRoute] Guid itemId,
                [FromBody] UpdateInStockCartItemRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInStockCartItemCommand(itemId, request.Quantity, request.InStockProductPriceDetailId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInStockCartItem")
            .WithSummary("[Customer]")
            .WithDescription("Updates the quantity of an item in the customer's InStock cart. User must be a customer.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))

            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateInStockCartItemRequest(
int? Quantity = null,
Guid? InStockProductPriceDetailId = null);
