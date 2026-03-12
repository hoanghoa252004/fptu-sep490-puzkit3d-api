using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductPriceDetails;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductPriceDetails;

internal sealed class InstockProductPriceDetailActivatedDomainEventHandler 
    : INotificationHandler<InstockProductPriceDetailActivatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductPriceDetailActivatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductPriceDetailActivatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        // Note: Activation event only has PriceDetailId and IsActive
        // For simplicity, we'll skip publishing this or fetch full data
        await Task.CompletedTask;
    }
}
