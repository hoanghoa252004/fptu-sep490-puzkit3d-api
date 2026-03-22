using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetAllPartnerProducts;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.GetAllPartnerProducts;

internal sealed class GetAllPartnerProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                Guid? partnerId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllPartnerProductsQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    partnerId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllPartnerProducts")
            .WithSummary("Get all partner products with pagination")
            .WithDescription("Retrieves a paginated list of partner products. Anonymous users and customers see only active products. Staff/Manager see all products with full details including IsActive, CreatedAt, and UpdatedAt.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
