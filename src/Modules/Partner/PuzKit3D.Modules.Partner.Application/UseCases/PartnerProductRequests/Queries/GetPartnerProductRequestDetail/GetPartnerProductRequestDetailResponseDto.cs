using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetPartnerProductRequestDetail;

public sealed record RequestDetailItemDto(
    Guid Id,
    Guid PartnerProductId,
    int Quantity,
    decimal ReferencePrice,
    decimal ReferenceTotalAmount);

public sealed record GetPartnerProductRequestDetailResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    int TotalRequestedQuantity,
    string? Note,
    PartnerProductRequestStatus Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<RequestDetailItemDto> Details);
