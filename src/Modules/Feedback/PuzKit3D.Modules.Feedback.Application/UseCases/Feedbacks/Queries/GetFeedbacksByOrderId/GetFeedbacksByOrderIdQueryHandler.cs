using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByOrderId;

internal sealed class GetFeedbackByOrderIdQueryHandler : IQueryHandler<GetFeedbackByOrderIdQuery, IEnumerable<FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly IOrderDetailReplicaRepository _orderDetailReplicaRepository;

    public GetFeedbackByOrderIdQueryHandler(
        IFeedbackRepository feedbackRepository,
        IOrderReplicaRepository orderReplicaRepository,
        IOrderDetailReplicaRepository orderDetailReplicaRepository)
    {
        _feedbackRepository = feedbackRepository;
        _orderReplicaRepository = orderReplicaRepository;
        _orderDetailReplicaRepository = orderDetailReplicaRepository;
    }

    public async Task<ResultT<IEnumerable<FeedbackDto>>> Handle(
        GetFeedbackByOrderIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if order exists
        var order = await _orderReplicaRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<IEnumerable<FeedbackDto>>(FeedbackError.OrderNotFound(request.OrderId));
        }

        IEnumerable<FeedbackDto> feedbackDtos;

        // If order type is not "Custom Design", get all feedbacks for all order details
        if (order.Type != "Custom Design")
        {
            var orderDetails = await _orderDetailReplicaRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            var orderDetailIds = orderDetails.Select(od => od.Id).ToList();

            if (!orderDetailIds.Any())
            {
                return Result.Success(Enumerable.Empty<FeedbackDto>());
            }

            var feedbacks = await _feedbackRepository.GetByOrderIdsAsync(orderDetailIds, cancellationToken);

            feedbackDtos = feedbacks.Select(feedback =>
            {
                var orderDetail = orderDetails.First(od => od.Id == feedback.OrderId);
                return new FeedbackDto(
                    feedback.Id.Value,
                    feedback.UserId,
                    feedback.Rating,
                    feedback.Comment,
                    feedback.CreatedAt,
                    feedback.UpdatedAt);
            }).ToList();
        }
        else
        {
            // For Custom Design orders, get the single feedback for the order
            var feedback = await _feedbackRepository.GetByOrderIdAsync(request.OrderId, cancellationToken);
            
            if (feedback is null)
            {
                return Result.Success(Enumerable.Empty<FeedbackDto>());
            }

            // For Custom Design, we get product info from the order detail associated with this order
            var orderDetail = (await _orderDetailReplicaRepository.GetByOrderIdAsync(request.OrderId, cancellationToken))
                .FirstOrDefault();

            var feedbackDto = new FeedbackDto(
                feedback.Id.Value,
                feedback.UserId,
                feedback.Rating,
                feedback.Comment,
                feedback.CreatedAt,
                feedback.UpdatedAt);

            feedbackDtos = new[] { feedbackDto };
        }

        return Result.Success(feedbackDtos);
    }
}
