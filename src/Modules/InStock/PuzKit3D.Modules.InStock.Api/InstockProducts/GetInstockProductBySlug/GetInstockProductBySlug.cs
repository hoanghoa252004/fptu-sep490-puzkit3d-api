using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockProducts.GetInstockProductBySlug;

internal sealed class GetInstockProductBySlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/slug/{slug}", async (
                string slug,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockProductBySlugQuery(slug);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockProductBySlug")
            .WithSummary("Get instock product by slug (Public)")
            .WithDescription("Retrieves an instock product by its slug. Anonymous/Customer users: only active products. Staff/Manager: all products (including inactive).")
            .AllowAnonymous()
            .Produces<GetInstockProductBySlugResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
