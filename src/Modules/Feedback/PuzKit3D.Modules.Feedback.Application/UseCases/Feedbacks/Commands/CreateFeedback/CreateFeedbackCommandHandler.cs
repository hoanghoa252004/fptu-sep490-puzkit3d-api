using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Commands.CreateFeedback;

internal sealed class CreateFeedbackCommandHandler : ICommandTHandler<CreateFeedbackCommand, Guid>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICompletedOrderReplicaRepository _orderReplicaRepository;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public CreateFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        ICompletedOrderReplicaRepository orderReplicaRepository,
        IFeedbackUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        // Check if order exists
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<Guid>(FeedbackError.OrderNotFoundOrNotCompleted(request.OrderId));
        }

        // Check if order belongs to the user
        if (order.CustomerId != request.UserId)
        {
            return Result.Failure<Guid>(FeedbackError.OrderNotBelongToUser(request.OrderId));
        }

        // Check if feedback already exists for this order and user
        var existingFeedback = await _feedbackRepository.GetByOrderIdAndUserIdAsync(
            request.OrderId,
            request.UserId,
            cancellationToken);

        if (existingFeedback is not null)
        {
            return Result.Failure<Guid>(FeedbackError.FeedbackAlreadyExists(request.OrderId, request.UserId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var feedbackResult = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.Feedback.Create(
                request.OrderId,
                request.UserId,
                request.Rating,
                request.Comment);

            if (feedbackResult.IsFailure)
            {
                return Result.Failure<Guid>(feedbackResult.Error);
            }

            var feedback = feedbackResult.Value;
            _feedbackRepository.Add(feedback);

            return Result.Success(feedback.Id.Value);
        }, cancellationToken);
    }
}
