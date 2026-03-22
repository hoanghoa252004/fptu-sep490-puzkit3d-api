using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.DeleteFeedback;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.DeleteFeedback;

internal sealed class DeleteFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapDelete("/feedbacks/{feedbackId}", async (
                [FromRoute] Guid feedbackId,
                ICurrentUser currentUser = null!,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var command = new DeleteFeedbackCommand(feedbackId, Guid.Parse(currentUser.UserId!));
                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(() => Results.NoContent());
            })
            .WithName("DeleteFeedback")
            .WithSummary("Delete a feedback")
            .WithDescription("Allows a customer to delete their feedback for an order.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status204NoContent);
    }
}
