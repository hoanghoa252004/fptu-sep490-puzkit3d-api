using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetAllInstockOrders;

public sealed record GetAllInstockOrdersResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    string? CustomerName,
    string? CustomerPhone,
    decimal GrandTotalAmount,
    int TotalQuantity,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime? PaidAt,
    DateTime CreatedAt,
    List<AllOrderDetailPreviewDto> OrderDetailsPreview);

public sealed record AllOrderDetailPreviewDto(
    string? ProductName,
    string? VariantName,
    int Quantity,
    decimal UnitPrice,
    string? ThumbnailUrl);
