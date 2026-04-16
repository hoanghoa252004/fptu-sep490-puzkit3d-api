using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.GetAllTopics;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.Topics.GetAllTopics;

internal sealed class GetAllTopics : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapTopicsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                bool ascending,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllTopicsQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive,
                    ascending);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllTopics")
            .WithSummary("Get all topics with pagination")
            .WithDescription("Retrieves a paginated list of topics. Anonymous users see only active items. Staff/Manager see all items with full details.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
