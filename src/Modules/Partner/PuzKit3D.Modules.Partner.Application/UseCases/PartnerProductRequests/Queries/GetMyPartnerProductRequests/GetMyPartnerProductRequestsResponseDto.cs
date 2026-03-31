namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetMyPartnerProductRequests;

public sealed record GetMyPartnerProductRequestsResponseDto(
    Guid Id,
    string Code,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    int TotalRequestedQuantity,
    string? Note,
    int Status,
    DateTime CreatedAt,
    DateTime UpdatedAt);
