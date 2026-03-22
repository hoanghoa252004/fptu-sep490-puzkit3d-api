using MediatR;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.Part;

internal class PartCreatedDomainEventHandler : INotificationHandler<PartCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public PartCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task Handle(PartCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new PartCreatedIntegrationEvent(
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
