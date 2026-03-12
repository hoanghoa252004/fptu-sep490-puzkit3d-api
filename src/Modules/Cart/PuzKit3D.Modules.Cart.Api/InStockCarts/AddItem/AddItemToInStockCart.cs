using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.AddItem;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Cart.Api.InStockCarts.AddItem;

internal sealed class AddItemToInStockCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapInStockCartsGroup()
            .MapPost("/items", async (
                [FromBody] AddItemToInStockCartRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new AddItemToInStockCartCommand(
                    request.ItemId, 
                    request.InStockProductPriceDetailId, 
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("AddItemToInStockCart")
            .WithSummary("[Customer]")
            .WithDescription("Adds an InStock product variant to the customer's InStock cart. User must be a customer.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))

            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record AddItemToInStockCartRequest(
    Guid ItemId,
    Guid InStockProductPriceDetailId,
    int? Quantity);
