using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Queries.GetInstockInventoryByVariantId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockInventories.GetInstockInventoryByVariantId;

internal sealed class GetInstockInventoryByVariantId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapInventoryGroup()
            .MapGet("", async (
                Guid productId,
                Guid variantId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockInventoryByVariantIdQuery(productId, variantId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockInventoryByVariantId")
            .WithSummary("Get inventory for a variant (Staff/Manager only)")
            .WithDescription("Retrieves inventory information for a specific product variant. Validates product and variant existence. Requires Staff or Manager role.")
            .AllowAnonymous()
            .Produces<GetInstockInventoryByVariantIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
