using MediatR;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Application.DomainEventHandlers.InstockPrices;

internal sealed class InstockPriceUpdatedDomainEventHandler 
    : INotificationHandler<InstockPriceUpdatedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;

    public InstockPriceUpdatedDomainEventHandler(
        IEventBus eventBus,
        IInstockProductPriceDetailRepository priceDetailRepository)
    {
        _eventBus = eventBus;
        _priceDetailRepository = priceDetailRepository;
    }

    public async Task Handle(
        InstockPriceUpdatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        // If price is being deactivated, deactivate all its price details
        // DbContext will automatically track these changes
        if (!domainEvent.IsActive)
        {
            var priceId = InstockPriceId.From(domainEvent.PriceId);
            var priceDetails = await _priceDetailRepository.GetAllByPriceIdAsync(priceId, cancellationToken);

            foreach (var priceDetail in priceDetails)
            {
                if (priceDetail.IsActive)
                {
                    priceDetail.PartialUpdate(isActive: false);
                    _priceDetailRepository.Update(priceDetail);
                }
            }
        }

        // Publish integration event
        var integrationEvent = new InstockPriceUpdatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.PriceId,
            domainEvent.Name,
            domainEvent.EffectiveFrom,
            domainEvent.EffectiveTo,
            domainEvent.Priority,
            domainEvent.IsActive);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}


