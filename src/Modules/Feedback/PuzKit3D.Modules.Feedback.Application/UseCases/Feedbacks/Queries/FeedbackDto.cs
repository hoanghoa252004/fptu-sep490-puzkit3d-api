namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries;

public sealed record FeedbackDto(
    Guid Id,
    Guid UserId,
    int Rating,
    string? Comment,
    DateTime CreatedAt,
    DateTime UpdatedAt);
