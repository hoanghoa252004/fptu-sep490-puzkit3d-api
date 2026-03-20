using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.CreateFeedback;

public sealed record CreateFeedbackCommand(
Guid OrderId,
Guid UserId,
int Rating,
string? Comment = null) : ICommandT<Guid>;
