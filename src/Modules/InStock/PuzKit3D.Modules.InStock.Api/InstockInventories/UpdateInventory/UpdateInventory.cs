using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Commands.UpdateInventory;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockInventories.UpdateInventory;

internal sealed class UpdateInventory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapInventoryGroup()
            .MapPut("", async (
                Guid productId,
                Guid variantId,
                [FromBody] UpdateInventoryRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInventoryCommand(productId, variantId, request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInventory")
            .WithSummary("Update variant inventory quantity (Staff/Manager only)")
            .WithDescription("Updates the inventory quantity for a variant. Creates new inventory if it doesn't exist. Validates product and variant existence. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateInventoryRequestDto(int Quantity);
