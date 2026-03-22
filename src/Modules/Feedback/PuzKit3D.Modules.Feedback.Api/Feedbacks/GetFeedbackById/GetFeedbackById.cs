using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.GetFeedbackById;

internal sealed class GetFeedbackById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapGet("/feedbacks/{feedbackId}", async (
                [FromRoute] Guid feedbackId,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetFeedbackByIdQuery(feedbackId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetFeedbackById")
            .Produces<PagedResult<FeedbackDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}