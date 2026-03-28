using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries.GetSupportTicketByOrderId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.SupportTicket.Api.SupportTickets.GetSupportTicketByOrderId;

internal sealed class GetSupportTicketByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapSupportTicketsGroup()
            .MapGet("/order/{orderId:guid}", async (
                Guid orderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetSupportTicketByOrderIdQuery(orderId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetSupportTicketByOrderId")
            .WithSummary("Get a support ticket by order ID (only 1 per order)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<SupportTicketDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
