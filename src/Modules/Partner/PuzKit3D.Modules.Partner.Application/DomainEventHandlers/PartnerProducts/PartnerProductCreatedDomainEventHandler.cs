using MediatR;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.PartnerProducts;

internal sealed class PartnerProductCreatedDomainEventHandler 
    : INotificationHandler<PartnerProductCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartnerProductCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(PartnerProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartnerProductCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.PartnerId,
            notification.Name,
            notification.ReferencePrice,
            notification.ThumbnailUrl,
            notification.PreviewAsset,
            notification.Slug,
            notification.Description,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
