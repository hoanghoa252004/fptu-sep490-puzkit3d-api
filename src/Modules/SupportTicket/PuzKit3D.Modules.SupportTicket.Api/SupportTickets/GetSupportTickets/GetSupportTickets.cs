using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;
using System.Security.Claims;

namespace PuzKit3D.Modules.SupportTicket.Api.SupportTickets.GetSupportTickets;

internal sealed class GetSupportTickets : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapSupportTicketsGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? status,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Validate and convert string status to enum
                string? parsedStatus = null;
                if (!string.IsNullOrEmpty(status))
                {
                    if (!Enum.TryParse<SupportTicketStatus>(status, ignoreCase: true, out var enumStatus))
                    {
                        return Results.BadRequest(new { error = $"Invalid status '{status}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(SupportTicketStatus)))}" });
                    }
                    parsedStatus = enumStatus.ToString();
                }

                var query = new GetSupportTicketsQuery(
                    pageNumber,
                    pageSize,
                    parsedStatus);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetSupportTickets")
            .WithSummary("Get support tickets with pagination and filtering")
            .WithDescription("Retrieves paginated support tickets. Customers see only their own tickets, Staff see all tickets. Can filter by status.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<PagedResult<SupportTicketDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

