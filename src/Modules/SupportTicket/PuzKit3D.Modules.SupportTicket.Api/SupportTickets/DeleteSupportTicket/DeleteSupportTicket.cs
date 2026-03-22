using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.DeleteSupportTicket;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Application.Authorization;
using System.Security.Claims;

namespace PuzKit3D.Modules.SupportTicket.Api.SupportTickets.DeleteSupportTicket;

internal sealed class DeleteSupportTicket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapSupportTicketsGroup()
            .MapDelete("/{id:guid}", async (
                Guid id,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteSupportTicketCommand(id);
                var result = await sender.Send(command, cancellationToken);

                return Results.Ok(result);
            })
            .WithName("DeleteSupportTicket")
            .WithSummary("Delete a support ticket")
            .WithDescription("Deletes a support ticket if its status is 'Open'. Customers can only delete their own tickets, Staff can delete any Open ticket.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces(200)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

