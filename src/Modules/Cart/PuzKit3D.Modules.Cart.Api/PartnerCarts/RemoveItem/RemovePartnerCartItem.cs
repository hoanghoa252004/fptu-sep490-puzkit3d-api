using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.RemoveItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.PartnerCarts.RemoveItem;

internal sealed class RemovePartnerCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerCartsGroup()
            .MapDelete("/items/{itemId:guid}", async (
                [FromRoute] Guid itemId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RemovePartnerCartItemCommand(itemId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("RemovePartnerCartItem")
            .WithSummary("Remove item from Partner cart")
            .WithDescription("Removes an item from the customer's Partner cart. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
