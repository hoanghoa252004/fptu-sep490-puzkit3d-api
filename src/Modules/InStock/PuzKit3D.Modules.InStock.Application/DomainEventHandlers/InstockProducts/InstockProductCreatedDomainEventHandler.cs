using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProducts;

internal sealed class InstockProductCreatedDomainEventHandler
    : INotificationHandler<InstockProductCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new InstockProductCreatedIntegrationEvent(
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
            notification.MaterialId,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
