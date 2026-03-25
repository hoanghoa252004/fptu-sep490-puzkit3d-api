using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackByOrderDetailId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.GetFeedbackByOrderDetailId;

internal sealed class GetFeedbackByOrderDetailId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapGet("/order-details/{orderDetailId}/feedback", async (
                [FromRoute] Guid orderDetailId,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetFeedbackByOrderDetailIdQuery(orderDetailId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(feedback =>
                    feedback is null ? Results.NoContent() : Results.Ok(feedback));
            })
            .WithName("GetFeedbackByOrderDetailId")
            .WithSummary("Get feedback for an order detail")
            .WithDescription("Retrieves the feedback for a specific order detail")
            .Produces<FeedbackDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}
