using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetStaffPartnerProductRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.GetStaffPartnerProductRequests;

internal sealed class GetStaffPartnerProductRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductRequestsGroup()
            .MapGet("/staff/requests", async (
                int pageNumber,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetStaffPartnerProductRequestsQuery(pageNumber, pageSize);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetStaffPartnerProductRequests")
            .WithSummary("Get partner product requests for staff (Pending, Approved, RejectedByStaff)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff))
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
