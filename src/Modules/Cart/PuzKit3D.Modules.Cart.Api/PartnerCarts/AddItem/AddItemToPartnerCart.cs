using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.AddItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Cart.Api.PartnerCarts.AddItem;

internal sealed class AddItemToPartnerCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerCartsGroup()
            .MapPost("/items", async (
                [FromBody] AddItemToPartnerCartRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new AddItemToPartnerCartCommand(request.ItemId, request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("AddItemToPartnerCart")
            .WithSummary("Add item to Partner cart")
            .WithDescription("Adds a Partner product to the customer's Partner cart. User must be a customer.")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}

internal sealed record AddItemToPartnerCartRequest(
    Guid ItemId,
    int? Quantity);
