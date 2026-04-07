namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetPartnerProductOrderById;

public sealed record GetPartnerProductOrderByIdResponseDto(
    Guid Id,
    Guid PartnerProductQuotationId,
    string Code,
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceName,
    string CustomerDistrictName,
    string CustomerWardName,
    decimal SubTotalAmount,
    decimal ShippingFee,
    decimal ImportTaxAmount,
    decimal GrandTotalAmount,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PaidAt,
    IReadOnlyList<PartnerProductOrderDetailDto> Details);

public sealed record PartnerProductOrderDetailDto(
    Guid Id,
    Guid PartnerProductId,
    string? PartnerProductName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalAmount);
