using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetAllInstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetAllInstockOrders;

internal sealed class GetAllInstockOrders : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapGet("/all", async (
                int pageNumber,
                int pageSize,
                string? status,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Validate and convert string status to enum
                InstockOrderStatus? parsedStatus = null;
                if (!string.IsNullOrEmpty(status))
                {
                    if (!Enum.TryParse<InstockOrderStatus>(status, ignoreCase: true, out var enumStatus))
                    {
                        return Results.BadRequest(new { error = $"Invalid status '{status}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(InstockOrderStatus)))}" });
                    }
                    parsedStatus = enumStatus;
                }

                var query = new GetAllInstockOrdersQuery(
                    pageNumber,
                    pageSize,
                    parsedStatus);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAllInstockOrders")
            .WithSummary("Get all instock orders with pagination and filter by status")
            .WithDescription("Retrieves a paginated list of all instock orders. Can filter by order status. Requires Staff or BusinessManager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<PagedResult<GetAllInstockOrdersResponseDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
