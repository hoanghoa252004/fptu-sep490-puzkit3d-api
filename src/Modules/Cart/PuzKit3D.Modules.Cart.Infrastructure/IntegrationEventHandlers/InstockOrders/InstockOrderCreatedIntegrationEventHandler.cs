using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;

    public InstockOrderCreatedIntegrationEventHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        if (@event.CartItemIds == null || !@event.CartItemIds.Any())
        {
            return;
        }

        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            @event.CustomerId,
            "INSTOCK",
            cancellationToken);

        if (cart is null)
        {
            return;
        }

        foreach (var itemId in @event.CartItemIds)
        {
            cart.RemoveItem(itemId);
        }

        _cartRepository.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
