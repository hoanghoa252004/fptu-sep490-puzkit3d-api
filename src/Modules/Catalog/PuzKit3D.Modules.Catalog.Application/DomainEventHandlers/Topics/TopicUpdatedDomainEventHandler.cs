using MediatR;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Topics;

internal sealed class TopicUpdatedDomainEventHandler
    : INotificationHandler<TopicUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public TopicUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(TopicUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new TopicUpdatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.TopicId,
            notification.Name,
            notification.Slug,
            notification.ParentId,
            notification.Description,
            notification.UpdatedAt,
            notification.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
