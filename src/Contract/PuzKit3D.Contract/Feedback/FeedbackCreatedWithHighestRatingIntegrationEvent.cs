using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Feedback;

public sealed record FeedbackCreatedWithHighestRatingIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid FeedbackId,
    Guid OrderId,
    Guid UserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt) : IIntegrationEvent;
