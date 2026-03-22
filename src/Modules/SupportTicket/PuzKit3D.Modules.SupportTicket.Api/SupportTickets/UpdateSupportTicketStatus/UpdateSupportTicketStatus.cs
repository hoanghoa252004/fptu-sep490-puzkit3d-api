using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.UpdateSupportTicketStatus;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.SupportTicket.Api.SupportTickets.UpdateSupportTicketStatus;

internal sealed class UpdateSupportTicketStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapSupportTicketsGroup()
            .MapPatch("/{id:guid}/status", async (
                Guid id,
                UpdateStatusRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (!Enum.TryParse<SupportTicketStatus>(request.Status, ignoreCase: true, out var status))
                {
                    return Results.BadRequest(new { error = $"Invalid status '{request.Status}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(SupportTicketStatus)))}" });
                }

                var command = new UpdateSupportTicketStatusCommand(id, status);
                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateSupportTicketStatus")
            .WithSummary("Update support ticket status")
            .WithDescription("Updates the status of a support ticket. Requires Staff role")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public sealed record UpdateStatusRequestDto(
    string Status);


