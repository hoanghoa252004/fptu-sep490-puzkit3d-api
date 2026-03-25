using MediatR;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProducts;

internal sealed class PartnerProductUpdatedDomainEventHandler
    : INotificationHandler<PartnerProductUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerProductUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PartnerProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.PartnerId,
            notification.Name,
            notification.ReferencePrice,
            notification.Quantity,
            notification.ThumbnailUrl,
            notification.PreviewAsset,
            notification.Slug,
            notification.Description,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}