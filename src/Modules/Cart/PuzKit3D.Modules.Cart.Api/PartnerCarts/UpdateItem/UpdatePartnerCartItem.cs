using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.UpdateItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.PartnerCarts.UpdateItem;

internal sealed class UpdatePartnerCartItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerCartsGroup()
            .MapPut("/items/{itemId:guid}", async (
                [FromRoute] Guid itemId,
                [FromBody] UpdatePartnerCartItemRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerCartItemCommand(itemId, request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdatePartnerCartItem")
            .WithSummary("Update Partner cart item quantity")
            .WithDescription("Updates the quantity of an item in the customer's Partner cart. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}

internal sealed record UpdatePartnerCartItemRequest(int Quantity);
