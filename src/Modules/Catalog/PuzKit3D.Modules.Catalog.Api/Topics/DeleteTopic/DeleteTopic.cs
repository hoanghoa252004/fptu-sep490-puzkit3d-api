using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.DeleteTopic;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Topics.DeleteTopic;

internal sealed class DeleteTopic : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapTopicsGroup()
            .MapDelete("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteTopicCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("DeleteTopic")
            .WithSummary("Delete a topic (Staff/Manager only)")
            .WithDescription("Deletes an existing topic. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
