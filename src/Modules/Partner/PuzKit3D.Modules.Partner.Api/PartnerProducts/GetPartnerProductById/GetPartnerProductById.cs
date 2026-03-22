using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.GetPartnerProductById;

internal sealed class GetPartnerProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapGet("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerProductByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerProductById")
            .WithSummary("Get a partner product by ID")
            .WithDescription("Retrieves a specific partner product by its ID. Anonymous users and customers only see active products. Staff/Manager see all products with full details.")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
