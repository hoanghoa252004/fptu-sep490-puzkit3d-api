using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductsByPartnerId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.GetPartnerProductsByPartnerId;

internal sealed class GetPartnerProductsByPartnerId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapGet("/{partnerId:guid}/products", async (
                Guid partnerId,
                int pageNumber,
                int pageSize,
                string? searchTerm,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerProductsByPartnerIdQuery(
                    partnerId,
                    pageNumber,
                    pageSize,
                    searchTerm);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerProductsByPartnerId")
            .WithSummary("Get all partner products by partner ID")
            .WithDescription("Retrieves a paginated list of products for a specific partner. Anonymous users and customers see only active products. Staff/Manager see all products with full details including IsActive, CreatedAt, and UpdatedAt.")
            .AllowAnonymous()
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
