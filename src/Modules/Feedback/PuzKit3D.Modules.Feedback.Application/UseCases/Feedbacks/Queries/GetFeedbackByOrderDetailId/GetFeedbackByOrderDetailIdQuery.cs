using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackByOrderDetailId;

public sealed record GetFeedbackByOrderDetailIdQuery(
    Guid OrderDetailId) : IQuery<FeedbackDto?>;
