namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetAllPartnerProductOrders;

public sealed record GetAllPartnerProductOrdersResponseDto(
    Guid Id,
    Guid PartnerProductQuotationId,
    string Code,
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    decimal SubTotalAmount,
    decimal ShippingFee,
    decimal ImportTaxAmount,
    decimal GrandTotalAmount,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime CreatedAt,
    DateTime UpdatedAt);
