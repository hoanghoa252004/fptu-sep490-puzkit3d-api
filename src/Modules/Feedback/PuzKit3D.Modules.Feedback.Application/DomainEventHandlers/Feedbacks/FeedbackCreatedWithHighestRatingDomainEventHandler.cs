using MediatR;
using PuzKit3D.Contract.Feedback;
using PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Application.DomainEventHandlers.Feedbacks;

internal sealed class FeedbackCreatedWithHighestRatingDomainEventHandler
    : INotificationHandler<FeedbackCreatedWithHighestRatingDomainEvent>
{
    private readonly IEventBus _eventBus;

    public FeedbackCreatedWithHighestRatingDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        FeedbackCreatedWithHighestRatingDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new FeedbackCreatedWithHighestRatingIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.FeedbackId,
            notification.OrderId,
            notification.UserId,
            notification.Rating,
            notification.Comment,
            notification.CreatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
