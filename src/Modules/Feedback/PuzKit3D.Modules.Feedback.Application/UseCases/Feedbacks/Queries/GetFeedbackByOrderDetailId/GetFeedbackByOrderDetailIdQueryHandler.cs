using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbackByOrderDetailId;

internal sealed class GetFeedbackByOrderDetailIdQueryHandler : IQueryHandler<GetFeedbackByOrderDetailIdQuery, FeedbackDto?>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;

    public GetFeedbackByOrderDetailIdQueryHandler(
        IFeedbackRepository feedbackRepository,
        IOrderDetailReplicaRepository orderDetailReplicaRepository)
    {
        _feedbackRepository = feedbackRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
    }

    public async Task<ResultT<FeedbackDto?>> Handle(
        GetFeedbackByOrderDetailIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if order detail exists
        var orderDetail = await _orderDetailReplicaRepository.GetByIdAsync(request.OrderDetailId, cancellationToken);
        if (orderDetail is null)
        {
            return Result.Failure<FeedbackDto?>(FeedbackError.OrderDetailNotFound(request.OrderDetailId));
        }

        // Get feedback for this order detail
        var feedback = await _feedbackRepository.GetByOrderIdAsync(request.OrderDetailId, cancellationToken);

        if (feedback is null)
        {
            return Result.Success<FeedbackDto?>(null);
        }

        var feedbackDto = new FeedbackDto(
            feedback.Id.Value,
            feedback.UserId,
            feedback.Rating,
            feedback.Comment,
            feedback.CreatedAt,
            feedback.UpdatedAt);

        return Result.Success<FeedbackDto?>(feedbackDto);
    }
}
