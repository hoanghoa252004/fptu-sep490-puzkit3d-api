using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetManagerPartnerProductRequests;

public sealed record GetManagerPartnerProductRequestsResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    int TotalRequestedQuantity,
    string? Note,
    PartnerProductRequestStatus Status,
    DateTime CreatedAt);
