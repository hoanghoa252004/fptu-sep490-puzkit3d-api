using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record ProductCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId,
    string Name,
    decimal Price,
    int InitialStock) : IIntegrationEvent;
