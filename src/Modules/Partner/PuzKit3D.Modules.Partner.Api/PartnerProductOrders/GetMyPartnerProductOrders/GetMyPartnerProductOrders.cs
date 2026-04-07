using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetMyPartnerProductOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductOrders.GetMyPartnerProductOrders;

internal sealed class GetMyPartnerProductOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductOrdersGroup()
            .MapGet("/my-orders", async (
                [FromQuery] int pageNumber,
                [FromQuery] int pageSize,
                [FromQuery] int? status,
                IHttpContextAccessor httpContextAccessor,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var customerId = httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;
                if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var customerGuid))
                {
                    return Results.Unauthorized();
                }

                pageNumber = pageNumber > 0 ? pageNumber : 1;
                pageSize = pageSize > 0 ? pageSize : 10;

                var query = new GetMyPartnerProductOrdersQuery(
                    customerGuid,
                    pageNumber,
                    pageSize,
                    status?.ToString());

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetMyPartnerProductOrders")
            .WithSummary("Get my partner product orders (Customer only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

