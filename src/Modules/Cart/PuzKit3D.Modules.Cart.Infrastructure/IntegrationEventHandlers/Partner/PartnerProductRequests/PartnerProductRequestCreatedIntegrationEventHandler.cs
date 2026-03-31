using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProductRequests;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProductRequests;

internal sealed class PartnerProductRequestCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductRequestCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;
    private readonly ICartUnitOfWork _unitOfWork;

    public PartnerProductRequestCreatedIntegrationEventHandler(
        CartDbContext context,
        ICartUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        PartnerProductRequestCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteAsync(async () =>
        {
            // Get list of product IDs (as ItemIds in Cart) from the request
            var productIds = @event.Items
                .Select(i => i.PartnerProductId)
                .ToList();

            // Get the customer's partner cart
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == @event.CustomerId && c.CartType == "PARTNER", cancellationToken);

            if (cart != null)
            {
                // Remove cart items that match the products in the request
                var rowsDeleted = await _context.CartItems
                    .Where(ci => ci.CartId == cart.Id && 
                                 productIds.Contains(ci.ItemId))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            return Result.Success();
        }, cancellationToken);
    }
}
