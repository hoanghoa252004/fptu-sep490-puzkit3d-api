using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Feedback.Application.UseCases.Feedbacks.Queries.GetFeedbacksByProductId;

internal sealed class GetFeedbacksByProductIdQueryHandler : IQueryHandler<GetFeedbacksByProductIdQuery, PagedResult<Queries.FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IProductReplicaRepository _productReplicaRepository;
    private readonly ICompletedOrderReplicaRepository _orderReplicaRepository;

    public GetFeedbacksByProductIdQueryHandler(
        IFeedbackRepository feedbackRepository,
        IProductReplicaRepository productReplicaRepository,
        ICompletedOrderReplicaRepository orderReplicaRepository)
    {
        _feedbackRepository = feedbackRepository;
        _productReplicaRepository = productReplicaRepository;
        _orderReplicaRepository = orderReplicaRepository;
    }

    public async Task<ResultT<PagedResult<Queries.FeedbackDto>>> Handle(
        GetFeedbacksByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if product exists
        var product = await _productReplicaRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result.Failure<PagedResult<Queries.FeedbackDto>>(
                FeedbackError.ProductNotFound());
        }

        var feedbacks = await _feedbackRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        
        // Get all orders for this product
        var orders = await _orderReplicaRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        var orderDict = orders.ToDictionary(o => o.Id);

        // Create list of feedbacks with product and variant info
        var feedbackList = feedbacks
            .Where(f => orderDict.ContainsKey(f.OrderId))
            .Select(f => new { Feedback = f, Order = orderDict[f.OrderId] })
            .ToList();

        var query = feedbackList.AsQueryable();

        // Apply rating filter if provided
        if (request.Rating.HasValue)
        {
            query = query.Where(x => x.Feedback.Rating == request.Rating.Value);
        }

        // Sort by CreatedAt descending (newest first)
        query = query.OrderByDescending(x => x.Feedback.CreatedAt);

        var totalCount = query.Count();

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new Queries.FeedbackDto(
                x.Feedback.Id.Value,
                x.Order.ProductId,
                x.Order.VariantId,
                x.Feedback.UserId,
                x.Feedback.Rating,
                x.Feedback.Comment,
                x.Feedback.CreatedAt,
                x.Feedback.UpdatedAt))
            .ToList();

        return Result.Success(new PagedResult<Queries.FeedbackDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount));
    }
}
