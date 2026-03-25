using MediatR;
using PuzKit3D.Contract.Partner.Partners;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Application.DomainEventHandlers.Partners;

internal sealed class PartnerDeletedDomainEventHandler
    : INotificationHandler<PartnerDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IPartnerProductRepository _productRepository;

    public PartnerDeletedDomainEventHandler(
        IEventBus eventBus,
        IPartnerProductRepository productRepository)
    {
        _eventBus = eventBus;
        _productRepository = productRepository;
    }

    public async Task Handle(
        PartnerDeletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _productRepository
        .DeactivateByPartnerIdAsync(notification.PartnerId, cancellationToken);


        var integrationEvent = new PartnerDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.PartnerId,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
