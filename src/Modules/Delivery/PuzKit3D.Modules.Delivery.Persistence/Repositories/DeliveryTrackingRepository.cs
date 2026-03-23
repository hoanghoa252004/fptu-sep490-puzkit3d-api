using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Persistence;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

/// <summary>
/// Repository implementation for DeliveryTracking aggregate
/// </summary>
internal sealed class DeliveryTrackingRepository : IDeliveryTrackingRepository
{
    private readonly DeliveryDbContext _dbContext;

    public DeliveryTrackingRepository(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeliveryTracking?> GetByIdAsync(
        DeliveryTrackingId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .FirstOrDefaultAsync(dt => dt.Id == id, cancellationToken);
    }

    public async Task<DeliveryTracking?> GetByDeliveryOrderCodeAsync(
        string deliveryOrderCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .FirstOrDefaultAsync(dt => dt.DeliveryOrderCode == deliveryOrderCode, cancellationToken);
    }

    public async Task<List<DeliveryTracking>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .Where(dt => dt.OrderId == orderId)
            .OrderByDescending(dt => dt.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DeliveryTracking>> GetByStatusAsync(
        DeliveryTrackingStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .Where(dt => dt.Status == status)
            .OrderByDescending(dt => dt.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DeliveryTracking>> GetByTypeAsync(
        DeliveryTrackingType type,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .Where(dt => dt.Type == type)
            .OrderByDescending(dt => dt.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<DeliveryTracking>> GetByOrderIdsAsync(
        IEnumerable<Guid> orderIds,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .Include(dt => dt.Details)
            .Where(dt => orderIds.Contains(dt.OrderId))
            .OrderByDescending(dt => dt.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Add(DeliveryTracking deliveryTracking)
    {
        _dbContext.DeliveryTrackings.Add(deliveryTracking);
    }

    public void Update(DeliveryTracking deliveryTracking)
    {
        _dbContext.DeliveryTrackings.Update(deliveryTracking);
    }

    public void Delete(DeliveryTracking deliveryTracking)
    {
        _dbContext.DeliveryTrackings.Remove(deliveryTracking);
    }

    public async Task<bool> ExistsAsync(
        string deliveryOrderCode,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.DeliveryTrackings
            .AnyAsync(dt => dt.DeliveryOrderCode == deliveryOrderCode, cancellationToken);
    }
}
