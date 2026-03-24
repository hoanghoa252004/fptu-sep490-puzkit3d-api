using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.DomainEvents;

public sealed record FeedbackCreatedWithHighestRatingDomainEvent(
    Guid FeedbackId,
    Guid OrderId,
    Guid UserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt) : DomainEvent;
