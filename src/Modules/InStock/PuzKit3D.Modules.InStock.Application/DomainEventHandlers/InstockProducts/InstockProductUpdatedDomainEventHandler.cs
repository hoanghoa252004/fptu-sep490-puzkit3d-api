using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProducts;

internal sealed class InstockProductUpdatedDomainEventHandler
    : INotificationHandler<InstockProductUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(InstockProductUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
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
            notification.CapabilityId,
            notification.MaterialId,
            notification.IsActive,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
