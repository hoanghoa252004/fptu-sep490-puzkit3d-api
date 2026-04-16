using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetAllDrives;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.Drives.GetAllDrives;

internal sealed class GetAllDrives : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDrivesGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                bool ascending,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllDrivesQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive,
                    ascending);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllDrives")
            .WithSummary("Get all drives with pagination")
            .WithDescription("Retrieves a paginated list of drives. Anonymous users see only active items. Staff/Manager see all items with full details. Sort by CreatedAt.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
