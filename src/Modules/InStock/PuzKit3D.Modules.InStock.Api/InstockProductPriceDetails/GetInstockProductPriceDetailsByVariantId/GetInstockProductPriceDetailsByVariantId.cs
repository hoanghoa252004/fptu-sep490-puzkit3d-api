using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailsByVariantId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.GetInstockProductPriceDetailsByVariantId;

internal sealed class GetInstockProductPriceDetailsByVariantId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapGet("/variant/{variantId:guid}", async (
                Guid variantId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockProductPriceDetailsByVariantIdQuery(variantId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockProductPriceDetailsByVariantId")
            .WithSummary("Get price details by product variant ID")
            .WithDescription("Staff/Manager: Returns all price details for the variant. Anonymous: Returns only the active price detail with the highest priority (1 is highest).")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
