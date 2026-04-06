using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetAllInstockProducts;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Api.InstockProducts.GetAllInstockProducts;

internal sealed class GetAllInstockProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                string? difficultyLevel,
                string? materialSlug,
                string? topicSlug,
                string? assemblyMethodSlug,
                string? capabilitySlugs,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Parse comma-separated capability slugs into a list
                var capabilitySlugsList = string.IsNullOrWhiteSpace(capabilitySlugs)
                    ? null
                    : capabilitySlugs.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList();

                var query = new GetAllInstockProductsQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive,
                    difficultyLevel,
                    materialSlug,
                    topicSlug,
                    assemblyMethodSlug,
                    capabilitySlugsList);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllInstockProducts")
            .WithSummary("Get all instock products with pagination and filters")
            .WithDescription("Retrieves a paginated list of instock products with optional filters. Anonymous/Customer users: only active products with limited fields. Staff/Manager: all products with full details including timestamps and IsActive flag. Filters: difficultyLevel, materialSlug, topicSlug, assemblyMethodSlug, capabilitySlugs (comma-separated).")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
