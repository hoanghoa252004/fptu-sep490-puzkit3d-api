namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetStaffPartnerProductRequests;

public sealed record GetStaffPartnerProductRequestsResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    int TotalRequestedQuantity,
    string? Note,
    int Status,
    DateTime CreatedAt);
