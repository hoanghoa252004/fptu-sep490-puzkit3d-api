using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.UpdateFeedback;

public sealed record UpdateFeedbackCommand(
    Guid FeedbackId,
    Guid UserId,
    int? Rating = null,
    string? Comment = null) : ICommand;
