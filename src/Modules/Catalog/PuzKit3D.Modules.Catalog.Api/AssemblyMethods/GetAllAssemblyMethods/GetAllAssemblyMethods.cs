using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.GetAllAssemblyMethods;

internal sealed class GetAllAssemblyMethods : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllAssemblyMethodsQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllAssemblyMethods")
            .WithSummary("Get all assembly methods with pagination")
            .WithDescription("Retrieves a paginated list of assembly methods with optional search and filtering")
            .AllowAnonymous()
            .Produces<PagedResult<GetAllAssemblyMethodsResponseDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
