using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetAllPartnerProductRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.GetAllPartnerProductRequests;

internal sealed class GetAllPartnerProductRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-requests")
            .WithTags("Partner Requests")
            .MapGet("/", async (
                int? status,
                DateTime? createdAtFrom,
                DateTime? createdAtTo,
                int pageNumber,
                int pageSize,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllPartnerProductRequestsQuery(
                    status,
                    createdAtFrom,
                    createdAtTo,
                    pageNumber,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllPartnerProductRequests")
            .WithSummary("Get all partner product requests (Staff/Manager only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
