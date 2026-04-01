using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetManagerPartnerProductRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.GetManagerPartnerProductRequests;

internal sealed class GetManagerPartnerProductRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductRequestsGroup()
            .MapGet("/manager/requests", async (
                string? searchTerm,
                int pageNumber,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetManagerPartnerProductRequestsQuery(searchTerm, pageNumber, pageSize);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetManagerPartnerProductRequests. Search by Code.")
            .WithSummary("Get partner product requests for manager (Approved, Quoted, Rejected, Cancelled)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.BusinessManager))
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
