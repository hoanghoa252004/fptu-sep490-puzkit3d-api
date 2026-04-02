using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationById;

public sealed record QuotationDetailItemDto(
    Guid Id,
    Guid PartnerProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalAmount);

public sealed record GetPartnerProductQuotationByIdResponseDto(
    Guid Id,
    string Code,
    Guid PartnerProductRequestId,
    decimal SubTotalAmount,
    decimal ShippingFee,
    decimal ImportTaxAmount,
    decimal GrandTotalAmount,
    string? Note,
    PartnerProductQuotationStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<QuotationDetailItemDto> Details);
