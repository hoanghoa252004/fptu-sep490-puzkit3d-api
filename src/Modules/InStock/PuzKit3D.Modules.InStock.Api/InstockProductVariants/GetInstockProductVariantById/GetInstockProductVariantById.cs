using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetInstockProductVariantById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductVariants.GetInstockProductVariantById;

internal sealed class GetInstockProductVariantById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapVariantsGroup()
            .MapGet("/{variantId:guid}", async (
                Guid productId,
                Guid variantId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockProductVariantByIdQuery(variantId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockProductVariantById")
            .WithSummary("Get an instock product variant by ID (Staff/Manager only)")
            .WithDescription("Retrieves a specific variant by its ID. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<GetInstockProductVariantByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
