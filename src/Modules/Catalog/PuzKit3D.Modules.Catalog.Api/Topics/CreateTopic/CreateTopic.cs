using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.CreateTopic;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Topics.CreateTopic;

internal sealed class CreateTopic : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapTopicsGroup()
            .MapPost("/", async (
                [FromBody] CreateTopicRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateTopicCommand(
                    request.Name,
                    request.Slug,
                    request.ParentId,
                    request.FactorPercentage,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetTopicById", id => new { id });
            })
            .WithName("CreateTopic")
            .WithSummary("Create a new topic (Staff/Manager only)")
            .WithDescription(
    "Creates a new topic with name, slug, parent ID, description, and active status. " +
    "ParentId must be null for a root topic. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateTopicRequestDto(
string Name,
string Slug,
Guid? ParentId,
decimal FactorPercentage,
string? Description,
bool IsActive);
