using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProducts;

internal sealed class InstockProductUpdatedDomainEventHandler
    : INotificationHandler<InstockProductUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInstockProductVariantRepository _variantRepository;

    public InstockProductUpdatedDomainEventHandler(
        IEventBus eventBus,
        IInstockProductVariantRepository variantRepository)
    {
        _eventBus = eventBus;
        _variantRepository = variantRepository;
    }

    public async Task Handle(InstockProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // If product is being deactivated, deactivate all its variants
        // DbContext will automatically track these changes
        if (!notification.IsActive)
        {
            var productId = InstockProductId.From(notification.ProductId);
            var variants = await _variantRepository.GetAllByProductIdAsync(productId, cancellationToken);

            foreach (var variant in variants)
            {
                if (variant.IsActive)
                {
                    variant.PartialUpdate(isActive: false);
                    _variantRepository.Update(variant);
                }
            }
        }

        // Publish integration event
        var integrationEvent = new InstockProductUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.ProductId,
            notification.Code,
            notification.Slug,
            notification.Name,
            notification.TotalPieceCount,
            notification.DifficultLevel,
            notification.EstimatedBuildTime,
            notification.ThumbnailUrl,
            notification.PreviewAsset,
            notification.Description,
            notification.TopicId,
            notification.AssemblyMethodId,
            notification.MaterialId,
            notification.IsActive,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}


