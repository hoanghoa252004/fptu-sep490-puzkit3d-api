using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantDeletedDomainEventHandler
    : INotificationHandler<InstockProductVariantDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductVariantDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductVariantDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductVariantDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.VariantId,
            notification.ProductId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
