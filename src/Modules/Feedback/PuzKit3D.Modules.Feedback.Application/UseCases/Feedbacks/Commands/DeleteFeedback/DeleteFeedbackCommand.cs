using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.DeleteFeedback;

public sealed record DeleteFeedbackCommand(
    Guid FeedbackId,
    Guid UserId) : ICommand;
