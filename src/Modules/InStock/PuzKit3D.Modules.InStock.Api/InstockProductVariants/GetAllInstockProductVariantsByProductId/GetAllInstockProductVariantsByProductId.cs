using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Queries.GetAllInstockProductVariantsByProductId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.InStock.Api.InstockProductVariants.GetAllInstockProductVariantsByProductId;

internal sealed class GetAllInstockProductVariantsByProductId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapVariantsGroup()
            .MapGet("/", async (
                Guid productId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllInstockProductVariantsByProductIdQuery(productId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllInstockProductVariantsByProductId")
            .WithSummary("Get all variants of an instock product")
            .WithDescription("Retrieves all variants for a specific instock product. Anonymous/Customer users: only active variants with full details. Staff/Manager: all variants including IsActive and timestamps.")
            .AllowAnonymous()
            .Produces<GetAllInstockProductVariantsByProductIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
