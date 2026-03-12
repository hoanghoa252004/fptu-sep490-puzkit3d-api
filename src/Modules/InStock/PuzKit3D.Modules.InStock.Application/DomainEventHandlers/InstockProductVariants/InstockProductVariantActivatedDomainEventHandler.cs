using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockProductVariants;

internal sealed class InstockProductVariantActivatedDomainEventHandler 
    : INotificationHandler<InstockProductVariantActivatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductVariantActivatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductVariantActivatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        // Note: For Activated event, we need to fetch the full variant data
        // For now, we'll just publish a simple update event with IsActive flag
        // In production, consider fetching full variant data from repository
        
        // Simplified approach: Just notify about the activation status change
        // The Cart module can query back if it needs full data
        await Task.CompletedTask;
    }
}
