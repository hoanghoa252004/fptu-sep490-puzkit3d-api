using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.SupportTicket.Api;
using PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Commands.CreateSupportTicket;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using System.Security.Claims;

namespace PuzKit3D.Modules.SupportTicket.Api.SupportTickets.CreateSupportTicket;

internal sealed class CreateSupportTicket : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapSupportTicketsGroup()
            .MapPost("/", async (
                [FromBody] CreateSupportTicketRequestDto request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                // Validate and convert string type to enum
                if (!Enum.TryParse<SupportTicketType>(request.Type, ignoreCase: true, out var typeEnum))
                {
                    return Results.BadRequest(new { error = $"Invalid type '{request.Type}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(SupportTicketType)))}" });
                }

                // Validate that at least one detail is provided
                if (request.Details == null || request.Details.Count == 0)
                {
                    return Results.BadRequest(new { error = "At least one support ticket detail is required" });
                }

                var details = request.Details
                    .Select(d => new CreateSupportTicketDetailDto(d.OrderDetailId, d.DriveId, d.Quantity, d.Note))
                    .ToList();

                var command = new CreateSupportTicketCommand(
                    userId,
                    request.OrderId,
                    typeEnum,
                    request.Reason,
                    request.Proof,
                    details);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("CreateSupportTicket")
            .WithSummary("Create a new support ticket")
            .WithDescription("Creates a new support ticket. Type should be one of: ReplacePart, Exchange, Return")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public sealed record CreateSupportTicketRequestDto(
    Guid OrderId,
    string Type,
    string Reason,
    string Proof,
    IReadOnlyList<CreateSupportTicketDetailRequestDto> Details);

public sealed record CreateSupportTicketDetailRequestDto(
Guid OrderDetailId,
Guid? DriveId,
int Quantity,
string? Note);

