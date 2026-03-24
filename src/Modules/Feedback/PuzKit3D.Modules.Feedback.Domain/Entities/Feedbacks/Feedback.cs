using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.DomainEvents;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;

public sealed class Feedback : AggregateRoot<FeedbackId>
{
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Feedback(
        FeedbackId id,
        Guid orderId,
        Guid userId,
        int rating,
        string? comment,
        DateTime createdAt) : base(id)
    {
        OrderId = orderId;
        UserId = userId;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Feedback() : base()
    {
    }

    public static ResultT<Feedback> Create(
        Guid orderId,
        Guid userId,
        int rating,
        string? comment = null,
        DateTime? createdAt = null)
    {
        if (orderId == Guid.Empty)
            return Result.Failure<Feedback>(FeedbackError.InvalidOrderId());

        if (userId == Guid.Empty)
            return Result.Failure<Feedback>(FeedbackError.InvalidUserId());

        if (rating < 1 || rating > 5)
            return Result.Failure<Feedback>(FeedbackError.InvalidRating());

        if (!string.IsNullOrWhiteSpace(comment) && comment.Length > 1000)
            return Result.Failure<Feedback>(FeedbackError.CommentTooLong());

        var now = createdAt ?? DateTime.UtcNow;

        var feedback = new Feedback(
            FeedbackId.Create(),
            orderId,
            userId,
            rating,
            comment,
            now);

        // Raise domain event if feedback has highest rating (5)
        if (rating == 5)
        {
            feedback.RaiseDomainEvent(new FeedbackCreatedWithHighestRatingDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                feedback.Id.Value,
                orderId,
                userId,
                rating,
                comment,
                now));
        }

        return Result.Success(feedback);
    }

    public Result Update(int? rating = null, string? comment = null)
    {
        // Validate rating only if provided
        if (rating.HasValue && (rating.Value < 1 || rating.Value > 5))
            return Result.Failure(FeedbackError.InvalidRating());

        // Validate comment only if provided
        if (!string.IsNullOrWhiteSpace(comment) && comment.Length > 1000)
            return Result.Failure(FeedbackError.CommentTooLong());

        // Update only provided fields
        if (rating.HasValue)
        {
            Rating = rating.Value;
        }

        if (comment != null) // Allow empty string to clear comment, but not null to keep old value
        {
            Comment = comment;
        }

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
