using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByProductId;

public sealed record GetFeedbacksByProductIdQuery(
    Guid ProductId,
    int? Rating = null,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<Queries.FeedbackDto>>;
