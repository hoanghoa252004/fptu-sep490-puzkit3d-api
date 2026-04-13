using MediatR;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.AssemblyMethods;

internal sealed class AssemblyMethodUpdatedDomainEventHandler
    : INotificationHandler<AssemblyMethodUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public AssemblyMethodUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(AssemblyMethodUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AssemblyMethodUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.AssemblyMethodId,
            notification.Name,
            notification.Slug,
            notification.FactorPercentage,
            notification.Description,
            notification.UpdatedAt,
            notification.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
