using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.DeleteFeedback;

internal sealed class DeleteFeedbackCommandHandler : ICommandHandler<DeleteFeedbackCommand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IFeedbackUnitOfWork _unitOfWork;
    private readonly IOrderDetailReplicaRepository _orderReplicaRepository;

    public DeleteFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IFeedbackUnitOfWork unitOfWork,
        IOrderDetailReplicaRepository orderReplicaRepository)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
        _orderReplicaRepository = orderReplicaRepository;
    }

    public async Task<Result> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Check if feedback exists
        var feedback = await _feedbackRepository.GetByIdAsync(
            FeedbackId.From(request.FeedbackId),
            cancellationToken);

        if (feedback is null)
        {
            return Result.Failure(FeedbackError.FeedbackNotFound(request.FeedbackId));
        }

        // Check authorization
        if (feedback.UserId != request.UserId)
        {
            return Result.Failure(FeedbackError.UnauthorizedAccess());
        }

        return await _unitOfWork.ExecuteAsync(() =>
        {
            _feedbackRepository.Delete(feedback);
            return Task.FromResult(Result.Success());
        }, cancellationToken);
    }
}
