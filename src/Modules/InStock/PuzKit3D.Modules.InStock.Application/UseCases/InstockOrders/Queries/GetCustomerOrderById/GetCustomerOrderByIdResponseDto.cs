using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrderById;

public sealed record GetCustomerOrderByIdResponseDto(
Guid Id,
string Code,
string CustomerName,
string CustomerPhone,
string CustomerEmail,
string CustomerProvinceName,
string CustomerDistrictName,
string CustomerWardName,
string DetailAddress,
decimal SubTotalAmount,
decimal ShippingFee,
int UsedCoinAmount,
decimal GrandTotalAmount,
string Status,
string PaymentMethod,
bool IsPaid,
DateTime? PaidAt,
DateTime CreatedAt,
DateTime UpdatedAt,
List<OrderDetailFullDto> OrderDetails);

public sealed record OrderDetailFullDto(
    Guid Id,
    Guid VariantId,
    string Sku,
    string? ProductName,
    string? VariantName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalAmount,
    string PriceName,
    string? ThumbnailUrl,
    ProductFullDetailsDto? ProductDetails,
    VariantFullDetailsDto? VariantDetails);

public sealed record ProductFullDetailsDto(
    Guid ProductId,
    string Code,
    string Name,
    string? Description,
    string DifficultLevel,
    int EstimatedBuildTime,
    int TotalPieceCount,
    string ThumbnailUrl,
    List<string> PreviewAsset,
    bool IsActive);

public sealed record VariantFullDetailsDto(
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    bool IsActive);


