using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetAllRequests;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequests.GetAllRequests;

internal sealed class GetAllCustomDesignRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequestGroup()
            .MapGet("/", async (
                int pageNumber = 1,
                int pageSize = 10,
                string? status = null,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetAllCustomDesignRequestsQuery(pageNumber, pageSize, status);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllCustomDesignRequests")
            .WithSummary("Get all custom design requests")
            .WithDescription("Gets all custom design requests. Customers can only see their own requests. Staff and Business Manager can see all requests. Supports pagination and filtering by status.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<PagedResult<GetAllCustomDesignRequestsResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}


