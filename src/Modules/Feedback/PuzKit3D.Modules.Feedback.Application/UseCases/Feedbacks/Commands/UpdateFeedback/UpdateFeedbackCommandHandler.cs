using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.UpdateFeedback;

internal sealed class UpdateFeedbackCommandHandler : ICommandHandler<UpdateFeedbackCommand>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public UpdateFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IOrderDetailReplicaRepository orderDetailReplicaRepository,
        IFeedbackUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
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
            var result = feedback.Update(request.Rating, request.Comment);
            if (result.IsFailure)
            {
                return Task.FromResult(result);
            }

            _feedbackRepository.Update(feedback);
            return Task.FromResult(Result.Success());
        }, cancellationToken);
    }
}
