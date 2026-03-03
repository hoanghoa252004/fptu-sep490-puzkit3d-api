using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.UpdateTopic;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Topics.UpdateTopic;

internal sealed class UpdateTopic : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapTopicsGroup()
            .MapPut("/{id}", async (
                Guid id,
                [FromBody] UpdateTopicRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateTopicCommand(
                    id,
                    request.Name,
                    request.Slug,
                    request.ParentId,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateTopic")
            .WithSummary("Update a topic (Staff/Manager only)")
            .WithDescription("Updates an existing topic with new name, slug, parent ID, description, and active status. Requires Staff or Manager permission.")
            .RequireAuthorization(Permissions.Catalog.ManageTopics)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateTopicRequestDto(
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    bool IsActive);
