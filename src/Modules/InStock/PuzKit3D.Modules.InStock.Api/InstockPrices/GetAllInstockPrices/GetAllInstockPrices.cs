using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetAllInstockPrices;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Api.InstockPrices.GetAllInstockPrices;

internal sealed class GetAllInstockPrices : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPricesGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                bool? isActive,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllInstockPricesQuery(
                    pageNumber,
                    pageSize,
                    searchTerm,
                    isActive);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllInstockPrices")
            .WithSummary("Get all instock prices with pagination (Staff/Manager only)")
            .WithDescription("Retrieves a paginated list of instock prices. Anonymous/Customer users: only active prices with limited fields. Staff/Manager: all prices with full details including timestamps and IsActive flag.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<PagedResult<object>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
