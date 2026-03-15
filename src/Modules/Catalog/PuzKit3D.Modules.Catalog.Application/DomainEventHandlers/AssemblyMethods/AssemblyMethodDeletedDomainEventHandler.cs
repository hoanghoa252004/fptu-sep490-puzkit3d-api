using MediatR;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.AssemblyMethods;

internal sealed class AssemblyMethodDeletedDomainEventHandler
    : INotificationHandler<AssemblyMethodDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public AssemblyMethodDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(AssemblyMethodDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AssemblyMethodDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.AssemblyMethodId,
            notification.DeletedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
