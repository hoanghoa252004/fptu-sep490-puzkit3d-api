using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public sealed record DeliveryTrackingDto(
    Guid Id,
    Guid OrderId,
    Guid? SupportTicketId,
    string DeliveryOrderCode,
    string Status,
    string Type,
    string? Note,
    DateTime ExpectedDeliveryDate,
    DateTime? DeliveredAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<DeliveryTrackingDetailDto> Details);

public sealed record DeliveryTrackingDetailDto(
    Guid Id,
    string Type,
    Guid ItemId,
    int Quantity);
