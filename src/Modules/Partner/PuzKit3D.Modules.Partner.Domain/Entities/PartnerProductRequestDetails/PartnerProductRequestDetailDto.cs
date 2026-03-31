namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;

public sealed record PartnerProductRequestDetailDto
(
    Guid PartnerProductId,
    int Quantity,
    decimal UnitPrice
);
