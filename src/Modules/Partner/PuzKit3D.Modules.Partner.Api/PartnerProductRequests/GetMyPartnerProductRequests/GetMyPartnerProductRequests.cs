using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetMyPartnerProductRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.GetMyPartnerProductRequests;

internal sealed class GetMyPartnerProductRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-requests")
            .WithTags("Partner Requests")
            .MapGet("/my-requests", async (
                int? status,
                DateTime? createdAtFrom,
                DateTime? createdAtTo,
                int pageNumber,
                int pageSize,
                ISender sender,
                IHttpContextAccessor httpContextAccessor,
                CancellationToken cancellationToken) =>
            {
                var customerId = httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var customerGuid))
                {
                    return Results.Unauthorized();
                }

                var query = new GetMyPartnerProductRequestsQuery(
                    customerGuid,
                    status,
                    createdAtFrom,
                    createdAtTo,
                    pageNumber,
                    pageSize);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetMyPartnerProductRequests")
            .WithSummary("Get my partner product requests (Customer only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
