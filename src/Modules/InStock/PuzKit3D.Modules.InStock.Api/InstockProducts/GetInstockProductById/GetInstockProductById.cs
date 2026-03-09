using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockProducts.GetInstockProductById;

internal sealed class GetInstockProductById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockProductByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockProductById")
            .WithSummary("Get instock product by ID (Public)")
            .WithDescription("Retrieves a specific instock product by its ID. Anonymous/Customer users: only active products. Staff/Manager: all products.")
            .AllowAnonymous()
            .Produces<GetInstockProductByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
