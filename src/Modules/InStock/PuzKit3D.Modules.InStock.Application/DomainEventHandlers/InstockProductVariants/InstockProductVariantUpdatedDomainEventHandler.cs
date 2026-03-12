using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantUpdatedDomainEventHandler 
    : INotificationHandler<InstockProductVariantUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductVariantUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductVariantUpdatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductVariantUpdatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.VariantId,
            domainEvent.ProductId,
            domainEvent.Sku,
            domainEvent.Color,
            domainEvent.AssembledLengthMm,
            domainEvent.AssembledWidthMm,
            domainEvent.AssembledHeightMm,
            domainEvent.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
