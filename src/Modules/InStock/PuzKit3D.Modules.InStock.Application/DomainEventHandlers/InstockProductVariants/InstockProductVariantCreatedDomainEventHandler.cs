using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantCreatedDomainEventHandler 
    : INotificationHandler<InstockProductVariantCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductVariantCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductVariantCreatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductVariantCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.VariantId,
            domainEvent.ProductId,
            domainEvent.Sku,
            domainEvent.Color,
            domainEvent.AssembledLengthMm,
            domainEvent.AssembledWidthMm,
            domainEvent.AssembledHeightMm,
            domainEvent.PreviewImages,
            domainEvent.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
