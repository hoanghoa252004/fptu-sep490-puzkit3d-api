using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

/// <summary>
/// Repository interface for DeliveryTracking aggregate
/// </summary>
public interface IDeliveryTrackingRepository
{
    /// <summary>
    /// Get delivery tracking by ID
    /// </summary>
    Task<DeliveryTracking?> GetByIdAsync(
        DeliveryTrackingId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get delivery tracking by GHN delivery order code
    /// </summary>
    Task<DeliveryTracking?> GetByDeliveryOrderCodeAsync(
        string deliveryOrderCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all delivery trackings for an order
    /// </summary>
    Task<List<DeliveryTracking>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get delivery trackings by status
    /// </summary>
    Task<List<DeliveryTracking>> GetByStatusAsync(
        DeliveryTrackingStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get delivery trackings by type
    /// </summary>
    Task<List<DeliveryTracking>> GetByTypeAsync(
        DeliveryTrackingType type,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all delivery trackings for multiple orders
    /// </summary>
    Task<List<DeliveryTracking>> GetByOrderIdsAsync(
        IEnumerable<Guid> orderIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get delivery trackings by support ticket ID
    /// </summary>
    Task<List<DeliveryTracking>> GetBySupportTicketIdAsync(
        Guid supportTicketId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new delivery tracking
    /// </summary>
    void Add(DeliveryTracking deliveryTracking);

    /// <summary>
    /// Update existing delivery tracking
    /// </summary>
    void Update(DeliveryTracking deliveryTracking);

    /// <summary>
    /// Delete delivery tracking
    /// </summary>
    void Delete(DeliveryTracking deliveryTracking);

    /// <summary>
    /// Check if delivery order code exists
    /// </summary>
    Task<bool> ExistsAsync(
        string deliveryOrderCode,
        CancellationToken cancellationToken = default);
}
