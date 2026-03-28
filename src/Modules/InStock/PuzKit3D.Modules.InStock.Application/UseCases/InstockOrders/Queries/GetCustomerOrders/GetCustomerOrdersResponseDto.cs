using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrders;

public sealed record GetCustomerOrdersResponseDto(
Guid Id,
string Code,
decimal GrandTotalAmount,
int TotalQuantity,
string Status,
string PaymentMethod,
bool IsPaid,
DateTime? PaidAt,
DateTime CreatedAt,
List<OrderDetailPreviewDto> OrderDetailsPreview);

public sealed record OrderDetailPreviewDto(
Guid ProductId,
string Slug,
string? ProductName,
string? VariantName,
int Quantity,
decimal UnitPrice,
string? ThumbnailUrl);

