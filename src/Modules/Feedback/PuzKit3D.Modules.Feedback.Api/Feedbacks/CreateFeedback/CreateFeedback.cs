using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.CreateFeedback;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.CreateFeedback;

internal sealed class CreateFeedback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapPost("/feedbacks", async (
                [FromBody] CreateFeedbackRequestDto request,
                ICurrentUser currentUser = null!,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var command = new CreateFeedbackCommand(
                    request.OrderId,
                    request.OrderDetailId,
                    Guid.Parse(currentUser.UserId!),
                    request.Rating,
                    request.Comment);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/feedbacks/{id}", id));
            })
            .WithName("CreateFeedback")
            .WithSummary("Create a feedback for an order")
            .WithDescription("Allows a customer to create a feedback for an order. Each customer can only create one feedback per order.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateFeedbackRequestDto(
Guid OrderId,
Guid? OrderDetailId,
int Rating,
string? Comment = null);
