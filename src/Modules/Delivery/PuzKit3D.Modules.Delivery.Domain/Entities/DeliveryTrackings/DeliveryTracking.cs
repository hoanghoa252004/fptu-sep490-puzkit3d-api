using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

public sealed class DeliveryTracking : AggregateRoot<DeliveryTrackingId>
{
    private readonly List<DeliveryTrackingDetail> _details = new();

    public Guid OrderId { get; private set; }
    public string DeliveryOrderCode { get; private set; } = null!;
    public DeliveryTrackingStatus Status { get; private set; }
    public DeliveryTrackingType Type { get; private set; }
    public string? Note { get; private set; }
    public DateTime ExpectedDeliveryDate { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Navigation property - Items trong delivery này
    /// </summary>
    public IReadOnlyCollection<DeliveryTrackingDetail> Details => _details.AsReadOnly();

    private DeliveryTracking(
        DeliveryTrackingId id,
        Guid orderId,
        string deliveryOrderCode,
        DeliveryTrackingStatus status,
        DeliveryTrackingType type,
        DateTime expectedDeliveryDate) : base(id)
    {
        OrderId = orderId;
        DeliveryOrderCode = deliveryOrderCode;
        Status = status;
        Type = type;
        ExpectedDeliveryDate = expectedDeliveryDate;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private DeliveryTracking() : base()
    {
    }

    public static ResultT<DeliveryTracking> Create(
        Guid orderId,
        string deliveryOrderCode,
        DateTime expectedDeliveryDate,
        DeliveryTrackingType type = DeliveryTrackingType.Original,
        string? note = null)
    {
        // Validation
        if (orderId == Guid.Empty)
            return Result.Failure<DeliveryTracking>(
                DeliveryTrackingError.InvalidOrderId());

        if (string.IsNullOrWhiteSpace(deliveryOrderCode))
            return Result.Failure<DeliveryTracking>(
                DeliveryTrackingError.InvalidDeliveryOrderCode());

        if (expectedDeliveryDate < DateTime.UtcNow)
            return Result.Failure<DeliveryTracking>(
                DeliveryTrackingError.InvalidExpectedDeliveryDate());

        var id = DeliveryTrackingId.NewId();
        var tracking = new DeliveryTracking(
            id,
            orderId,
            deliveryOrderCode,
            DeliveryTrackingStatus.ReadyToPick,
            type,
            expectedDeliveryDate)
        {
            Note = note
        };

        return Result.Success(tracking);
    }

    public Result AddDetail(DeliveryTrackingDetail detail)
    {
        if (detail == null)
            return Result.Failure(DeliveryTrackingError.InvalidDetail());

        if (_details.Any(d => d.ItemId == detail.ItemId))
            return Result.Failure(DeliveryTrackingError.ItemAlreadyExists(detail.ItemId));

        _details.Add(detail);
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result AddDetails(IEnumerable<DeliveryTrackingDetail> details)
    {
        foreach (var detail in details)
        {
            var result = AddDetail(detail);
            if (result.IsFailure)
                return result;
        }

        return Result.Success();
    }

    public Result MarkAsPicked()
    {
        if (Status != DeliveryTrackingStatus.ReadyToPick)
            return Result.Failure(
                DeliveryTrackingError.InvalidStatusTransition(Status, DeliveryTrackingStatus.Picked));

        Status = DeliveryTrackingStatus.Picked;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsShipping()
    {
        if (Status != DeliveryTrackingStatus.Picked)
            return Result.Failure(
                DeliveryTrackingError.InvalidStatusTransition(Status, DeliveryTrackingStatus.Shipping));

        Status = DeliveryTrackingStatus.Shipping;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsDelivered(DateTime? deliveredAt = null)
    {
        if (Status != DeliveryTrackingStatus.Shipping)
            return Result.Failure(
                DeliveryTrackingError.InvalidStatusTransition(Status, DeliveryTrackingStatus.Delivered));

        DeliveredAt = deliveredAt ?? DateTime.UtcNow;
        Status = DeliveryTrackingStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsReturn(string? reason = null)
    {
        if (Status == DeliveryTrackingStatus.Returned)
            return Result.Failure(DeliveryTrackingError.AlreadyReturned());

        Status = DeliveryTrackingStatus.Return;
        if (!string.IsNullOrWhiteSpace(reason))
            Note = reason;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsReturned(DateTime? returnedAt = null)
    {
        if (Status != DeliveryTrackingStatus.Return)
            return Result.Failure(
                DeliveryTrackingError.InvalidStatusTransition(Status, DeliveryTrackingStatus.Returned));

        DeliveredAt = returnedAt ?? DateTime.UtcNow;
        Status = DeliveryTrackingStatus.Returned;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void UpdateNote(string? note)
    {
        Note = note;
        UpdatedAt = DateTime.UtcNow;
    }

    public int GetTotalQuantity() => _details.Sum(d => d.Quantity);

    public bool IsCompleted() =>
        Status == DeliveryTrackingStatus.Delivered || Status == DeliveryTrackingStatus.Returned;
}
