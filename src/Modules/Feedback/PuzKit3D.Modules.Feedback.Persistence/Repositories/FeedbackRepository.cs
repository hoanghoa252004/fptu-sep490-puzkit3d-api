using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using FeedbackEntity = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.Feedback;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Feedback.Persistence.Repositories;

internal sealed class FeedbackRepository : IFeedbackRepository
{
    private readonly FeedbackDbContext _context;

    public FeedbackRepository(FeedbackDbContext context)
    {
        _context = context;
    }

    public async Task<FeedbackEntity?> GetByIdAsync(
        FeedbackId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<FeedbackEntity?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.OrderId == orderId, cancellationToken);
    }

    public async Task<FeedbackEntity?> GetByOrderIdAndUserIdAsync(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .FirstOrDefaultAsync(f => f.OrderId == orderId && f.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<FeedbackEntity>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .Where(f => f.UserId == userId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FeedbackEntity>> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var orderIds = await _context.CompletedOrderReplicas
            .Where(o => o.ProductId == productId)
            .Select(o => o.Id)
            .ToListAsync(cancellationToken);

        return await _context.Feedbacks
            .Where(f => orderIds.Contains(f.OrderId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FeedbackEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FeedbackEntity>> FindAsync(
        Expression<Func<FeedbackEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(FeedbackEntity entity)
    {
        _context.Feedbacks.Add(entity);
    }

    public void Update(FeedbackEntity entity)
    {
        _context.Feedbacks.Update(entity);
    }

    public void Delete(FeedbackEntity entity)
    {
        _context.Feedbacks.Remove(entity);
    }

    public void DeleteMultiple(List<FeedbackEntity> entities)
    {
        _context.Feedbacks.RemoveRange(entities);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
