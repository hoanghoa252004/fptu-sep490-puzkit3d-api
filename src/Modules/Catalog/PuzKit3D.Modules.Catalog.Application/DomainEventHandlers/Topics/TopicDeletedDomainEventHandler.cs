using MediatR;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Topics;

internal sealed class TopicDeletedDomainEventHandler
    : INotificationHandler<TopicDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public TopicDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(TopicDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new TopicDeletedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.TopicId,
            notification.DeletedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
