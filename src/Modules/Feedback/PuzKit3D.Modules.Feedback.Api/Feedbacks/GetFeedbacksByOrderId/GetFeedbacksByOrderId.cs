using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByOrderId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.GetFeedbacksByOrderId;

internal sealed class GetFeedbackByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapGet("/orders/{orderId}/feedback", async (
                [FromRoute] Guid orderId,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetFeedbackByOrderIdQuery(orderId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(feedback =>
                    Results.Ok(feedback));
            })
            .WithName("GetFeedbackByOrderId")
            .WithSummary("Get feedback for an order")
            .WithDescription("Retrieves the feedback for a specific order. Nếu là instock hoặc partner thì orderId truyền vào là orderDetailId, nếu là custom design thì là orderId truyền vào là orderId gốc")
            .WithOpenApi()
            .Produces<FeedbackDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}


