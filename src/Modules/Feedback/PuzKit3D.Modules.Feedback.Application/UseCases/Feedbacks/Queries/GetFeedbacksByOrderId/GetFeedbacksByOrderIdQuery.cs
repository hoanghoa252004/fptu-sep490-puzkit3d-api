using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByOrderId;

public sealed record GetFeedbackByOrderIdQuery(
    Guid OrderId) : IQuery<IEnumerable<FeedbackDto>>;
