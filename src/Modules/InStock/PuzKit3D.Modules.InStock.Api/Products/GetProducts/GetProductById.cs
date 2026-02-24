using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.Products.GetProducts;

internal sealed class GetProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query, ct);

                return result.MatchOk();
            })
            .WithName("GetProductById")
            .WithSummary("Get a product by ID")
            .WithDescription("Retrieves a single product by its unique identifier")
            .AllowAnonymous()
            .Produces<GetProductByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
