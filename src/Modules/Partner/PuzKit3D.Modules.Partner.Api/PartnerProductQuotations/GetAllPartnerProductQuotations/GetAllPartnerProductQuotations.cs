using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetAllPartnerProductQuotations;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductQuotations.GetAllPartnerProductQuotations;

internal sealed class GetAllPartnerProductQuotations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductQuotationsGroup()
            .MapGet("/", async (
                DateTime? createdAtFrom,
                DateTime? createdAtTo,
                bool ascending,
                int pageNumber,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllPartnerProductQuotationsQuery(
                    createdAtFrom,
                    createdAtTo,
                    ascending,
                    pageNumber,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllPartnerProductQuotations")
            .WithSummary("Get all partner product quotations (Manager only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.BusinessManager))
            .Produces<PagedResult<GetAllPartnerProductQuotationsResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
