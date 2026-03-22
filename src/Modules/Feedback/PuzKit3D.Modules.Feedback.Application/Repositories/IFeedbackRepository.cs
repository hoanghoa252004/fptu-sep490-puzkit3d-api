using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using FeedbackEntity = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.Feedback;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Feedback.Application.Repositories;

public interface IFeedbackRepository : IRepositoryBase<FeedbackEntity, FeedbackId>
{
    Task<FeedbackEntity?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<FeedbackEntity>> GetByOrderIdsAsync(IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default);
    
    Task<FeedbackEntity?> GetByOrderIdAndUserIdAsync(Guid orderId, Guid userId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<FeedbackEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<FeedbackEntity>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
