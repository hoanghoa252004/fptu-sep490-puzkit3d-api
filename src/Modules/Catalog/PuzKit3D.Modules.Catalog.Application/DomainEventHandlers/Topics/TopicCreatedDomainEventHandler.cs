using MediatR;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Application.DomainEventHandlers.Topics;

internal sealed class TopicCreatedDomainEventHandler
    : INotificationHandler<TopicCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public TopicCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(TopicCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new TopicCreatedIntegrationEvent(
            Guid.NewGuid(),
            notification.OccurredOn,
            notification.TopicId,
            notification.Name,
            notification.Slug,
            notification.ParentId,
            notification.FactorPercentage,
            notification.Description,
            notification.IsActive,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
