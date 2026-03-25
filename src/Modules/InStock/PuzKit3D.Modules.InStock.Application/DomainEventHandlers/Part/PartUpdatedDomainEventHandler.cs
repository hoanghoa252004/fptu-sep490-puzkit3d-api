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

internal class PartUpdatedDomainEventHandler : INotificationHandler<PartUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartUpdatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task Handle(PartUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartUpdatedIntegrationEvent(
            notification.Id,
            notification.OccurredOn,
            notification.PartId,
            notification.Name,
            notification.PartType,
            notification.Code,
            notification.Quantity,
            notification.InstockProductId);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}

