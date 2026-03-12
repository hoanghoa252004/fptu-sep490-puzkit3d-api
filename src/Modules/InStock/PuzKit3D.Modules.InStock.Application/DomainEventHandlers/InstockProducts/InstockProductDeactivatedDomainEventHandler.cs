using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProducts;

internal sealed class InstockProductDeactivatedDomainEventHandler
    : INotificationHandler<InstockProductDeactivatedDomainEvent>
{
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IEventBus _eventBus;

    public InstockProductDeactivatedDomainEventHandler(
        IInstockProductVariantRepository variantRepository,
        IEventBus eventBus)
    {
        _variantRepository = variantRepository;
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductDeactivatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(notification.ProductId);

        var variants = await _variantRepository.GetAllByProductIdAsync(productId, cancellationToken);

        foreach (var variant in variants)
        {
            if (variant.IsActive)
            {
                variant.Deactivate();
                _variantRepository.Update(variant);

                var integrationEvent = new InstockProductVariantDeactivatedIntegrationEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    variant.Id.Value,
                    notification.ProductId);

                await _eventBus.PublishAsync(integrationEvent, cancellationToken);
            }
        }
    }
}
