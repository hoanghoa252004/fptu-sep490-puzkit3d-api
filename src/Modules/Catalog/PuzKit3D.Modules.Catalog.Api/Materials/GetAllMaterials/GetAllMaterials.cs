using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetAllMaterials;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.Materials.GetAllMaterials;

internal sealed class GetAllMaterials : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMaterialsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                bool ascending,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllMaterialsQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive,
                    ascending);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllMaterials")
            .WithSummary("Get all materials with pagination")
            .WithDescription("Retrieves a paginated list of materials. Anonymous users see only active items. Staff/Manager see all items with full details.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
