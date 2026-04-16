using MediatR;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.AssemblyMethods;

internal sealed class AssemblyMethodCreatedDomainEventHandler
    : INotificationHandler<AssemblyMethodCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public AssemblyMethodCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(AssemblyMethodCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AssemblyMethodCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.AssemblyMethodId,
            notification.Name,
            notification.Slug,
            notification.FactorPercentage,
            notification.Description,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
