using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries;
using PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByProductId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Feedback.Api.Feedbacks.GetFeedbacksByProductId;

internal sealed class GetFeedbacksByProductId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFeedbacksGroup()
            .MapGet("/products/{productId}/feedbacks", async (
                [FromRoute] Guid productId,
                [FromQuery] int? rating = null,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                ISender sender = null!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetFeedbacksByProductIdQuery(productId, rating, pageNumber, pageSize);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk(pagedResult =>
                    Results.Ok(new
                    {
                        data = pagedResult.Items,
                        pagination = new
                        {
                            pageNumber = pagedResult.PageNumber,
                            pageSize = pagedResult.PageSize,
                            totalCount = pagedResult.TotalCount,
                            totalPages = pagedResult.TotalPages
                        }
                    }));
            })
            .WithName("GetFeedbacksByProductId")
            .WithSummary("Get all feedbacks for a product")
            .WithDescription("Retrieves all feedbacks for a specific product with optional rating filter and pagination.")
            .WithOpenApi()
            .Produces<PagedResult<FeedbackDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .AllowAnonymous();
    }
}

