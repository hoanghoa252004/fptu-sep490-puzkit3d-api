namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetAllPartnerProductRequests;

public sealed record GetAllPartnerProductRequestsResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    int TotalRequestedQuantity,
    string? Note,
    int Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
