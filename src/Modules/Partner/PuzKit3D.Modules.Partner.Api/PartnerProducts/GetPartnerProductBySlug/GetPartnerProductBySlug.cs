using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductBySlug;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.GetPartnerProductBySlug;

internal sealed class GetPartnerProductBySlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapGet("/slug/{slug}", async (
                string slug,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerProductBySlugQuery(slug);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerProductBySlug")
            .WithSummary("Get a partner product by slug")
            .WithDescription("Retrieves a specific partner product by its slug. Anonymous users and customers only see active products. Staff/Manager see all products with full details.")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
