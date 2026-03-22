using MediatR;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.Part;

internal class PartDeletedDomainEventHandler : INotificationHandler<PartDeletedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartDeletedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task Handle(PartDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartDeletedIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.PartId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}