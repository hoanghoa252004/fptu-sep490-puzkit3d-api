using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetAllPartnerProductQuotations;

public sealed record GetAllPartnerProductQuotationsResponseDto(
    Guid Id,
    string Code,
    Guid PartnerProductRequestId,
    int Version,
    decimal SubTotalAmount,
    decimal ShippingFee,
    decimal ImportTaxAmount,
    decimal GrandTotalAmount,
    DateTime ExpectedDeliveryDate,
    string? Note,
    PartnerProductQuotationStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
