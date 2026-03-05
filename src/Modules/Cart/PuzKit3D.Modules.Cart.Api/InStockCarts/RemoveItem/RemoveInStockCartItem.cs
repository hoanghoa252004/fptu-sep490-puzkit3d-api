using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.RemoveItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Cart.Api.InStockCarts.RemoveItem;

internal sealed class RemoveInStockCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapInStockCartsGroup()
            .MapDelete("/items/{itemId:guid}", async (
                [FromRoute] Guid itemId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RemoveInStockCartItemCommand(itemId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("RemoveInStockCartItem")
            .WithSummary("[Customer]")
            .WithDescription("Removes an item from the customer's InStock cart. User must be a customer.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))

            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
