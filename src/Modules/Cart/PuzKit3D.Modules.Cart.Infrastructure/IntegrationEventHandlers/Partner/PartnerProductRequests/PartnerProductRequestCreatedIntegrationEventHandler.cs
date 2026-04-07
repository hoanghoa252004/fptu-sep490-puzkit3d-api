using PuzKit3D.Contract.Partner.PartnerProductRequests;
using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProductRequests;

internal sealed class PartnerProductRequestCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductRequestCreatedIntegrationEvent>
{
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;

    public PartnerProductRequestCreatedIntegrationEventHandler(
        ICartUnitOfWork unitOfWork,
        ICartRepository cartRepository)
    {
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }

    public async Task HandleAsync(
        PartnerProductRequestCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        if (@event.Items == null || !@event.Items.Any())
        {
            return;
        }

        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            @event.CustomerId,
            "PARTNER",
            cancellationToken);

        if (cart is null)
        {
            return;
        }

        foreach (var itemId in @event.Items.Select(i => i.PartnerProductId))
        {
            cart.RemoveItem(itemId);
        }

        _cartRepository.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
