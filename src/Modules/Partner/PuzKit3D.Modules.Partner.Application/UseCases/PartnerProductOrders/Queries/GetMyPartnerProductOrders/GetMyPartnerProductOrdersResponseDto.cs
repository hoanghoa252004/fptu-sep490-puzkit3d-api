namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetMyPartnerProductOrders;

public sealed record GetMyPartnerProductOrdersResponseDto(
    Guid Id,
    string Code,
    decimal SubTotalAmount,
    decimal ShippingFee,
    decimal ImportTaxAmount,
    decimal GrandTotalAmount,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime CreatedAt,
    DateTime UpdatedAt);
