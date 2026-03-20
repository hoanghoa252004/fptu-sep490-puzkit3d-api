using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByOrderId;

internal sealed class GetFeedbackByOrderIdQueryHandler : IQueryHandler<GetFeedbackByOrderIdQuery, FeedbackDto>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICompletedOrderReplicaRepository _orderReplicaRepository;

    public GetFeedbackByOrderIdQueryHandler(
        IFeedbackRepository feedbackRepository,
        ICompletedOrderReplicaRepository orderReplicaRepository)
    {
        _feedbackRepository = feedbackRepository;
        _orderReplicaRepository = orderReplicaRepository;
    }

    public async Task<ResultT<FeedbackDto>> Handle(
        GetFeedbackByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if order exists
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<FeedbackDto>(
                FeedbackError.OrderNotFoundOrNotCompleted(request.OrderId));
        }

        var feedback = await _feedbackRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
        if (feedback is null)
        {
            return Result.Failure<FeedbackDto>(
                FeedbackError.FeedbackNotFoundForOrder(request.OrderId));
        }

        var feedbackDto = new FeedbackDto(
            feedback.Id.Value,
            order.ProductId,
            order.VariantId,
            feedback.UserId,
            feedback.Rating,
            feedback.Comment,
            feedback.CreatedAt,
            feedback.UpdatedAt);

        return Result.Success(feedbackDto);
    }
}
