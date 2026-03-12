using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.InstockPrices;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockPrices;

internal sealed class InstockPriceActivatedDomainEventHandler 
    : INotificationHandler<InstockPriceActivatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockPriceActivatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockPriceActivatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        // Note: Activation event only has PriceId and IsActive
        // For simplicity, we'll skip publishing this or fetch full data
        await Task.CompletedTask;
    }
}
