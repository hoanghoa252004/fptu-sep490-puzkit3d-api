using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Contract.InStock.Part;

public sealed record PartDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PartId) : IIntegrationEvent;
