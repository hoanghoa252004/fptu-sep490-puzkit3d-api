using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Contract.InStock.Part;

public sealed record PartCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PartId,
    string Name,
    string PartType,
    string Code,
    int Quantity,
    Guid InstockProductId) : IIntegrationEvent;
