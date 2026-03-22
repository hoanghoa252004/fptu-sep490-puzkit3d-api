using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetAllPartners;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.Partners.GetAllPartners;

internal sealed class GetAllPartners : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllPartnersQuery(
                    pageNumber,
                    pageSize,
                    searchTerm);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllPartners")
            .WithSummary("Get all partners with pagination")
            .WithDescription("Retrieves a paginated list of partners. Anonymous users and customers see only active partners. Staff/Manager see all partners with full details including IsActive, CreatedAt, and UpdatedAt.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
