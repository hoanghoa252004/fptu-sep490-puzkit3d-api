using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;

public static class FeedbackError
{
    public static Error InvalidOrderId() =>
        Error.Validation("Feedback.InvalidOrderId", "Order ID cannot be empty.");

    public static Error InvalidUserId() =>
        Error.Validation("Feedback.InvalidUserId", "User ID cannot be empty.");

    public static Error InvalidRating() =>
        Error.Validation("Feedback.InvalidRating", "Rating must be between 1 and 5.");

    public static Error CommentTooLong() =>
        Error.Validation("Feedback.CommentTooLong", "Comment cannot exceed 1000 characters.");

    public static Error FeedbackNotFound(Guid feedbackId) =>
        Error.NotFound("Feedback.FeedbackNotFound", $"Feedback with ID '{feedbackId}' was not found.");

    public static Error FeedbackNotFoundForOrder(Guid orderId) =>
        Error.NotFound("Feedback.FeedbackNotFoundForOrder", $"No feedback found for order '{orderId}'.");

    public static Error OrderNotFoundOrNotCompleted(Guid orderId) =>
        Error.NotFound("Feedback.OrderNotFoundOrNotCompleted", $"Order with ID '{orderId}' was not found or not complete to create feedback.");

    public static Error FeedbackAlreadyExists(Guid orderId, Guid userId) =>
        Error.Conflict("Feedback.FeedbackAlreadyExists", $"Feedback for order '{orderId}' from user '{userId}' already exists.");

    public static Error UnauthorizedAccess() =>
        Error.Forbidden("Feedback.UnauthorizedAccess", "You do not have permission to perform this action.");

    public static Error OrderNotBelongToUser(Guid orderId) =>
        Error.Forbidden("Feedback.OrderNotBelongToUser", $"Order with ID '{orderId}' does not belong to you.");

    public static Error ProductNotFound() =>
        Error.NotFound("Feedback.ProductNotFound", "Product was not found.");
}
