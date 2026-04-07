using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductRequests;

public sealed record PartnerProductRequestCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PartnerProductRequestId,
    Guid CustomerId,
    Guid PartnerId,
    List<PartnerProductRequestItemDto> Items,
    DateTime CreatedAt
) : IIntegrationEvent;

public sealed record PartnerProductRequestItemDto(
    Guid PartnerProductId);