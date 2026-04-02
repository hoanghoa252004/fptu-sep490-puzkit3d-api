using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationByRequestId;

public sealed record QuotationDetailItemByIdDto(
    Guid Id,
    Guid PartnerProductId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalAmount);

public sealed record GetPartnerProductQuotationByRequestIdResponseDto(
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
    IEnumerable<QuotationDetailItemByIdDto> Details);
