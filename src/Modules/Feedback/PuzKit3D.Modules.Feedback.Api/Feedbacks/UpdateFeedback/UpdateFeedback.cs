using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.UpdateFeedback;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.UpdateFeedback;

internal sealed class UpdateFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapPut("/feedbacks/{feedbackId}", async (
                [FromRoute] Guid feedbackId,
                [FromBody] UpdateFeedbackRequestDto request,
                ICurrentUser currentUser = null!,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var command = new UpdateFeedbackCommand(
                    feedbackId,
                    Guid.Parse(currentUser.UserId!),
                    request.Rating,
                    request.Comment);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(() => Results.NoContent());
            })
            .WithName("UpdateFeedback")
            .WithSummary("Update a feedback")
            .WithDescription("Allows a customer to update their feedback for an order. Only the rating and comment can be updated.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status204NoContent);
    }
}

internal sealed record UpdateFeedbackRequestDto(
int? Rating = null,
string? Comment = null);
