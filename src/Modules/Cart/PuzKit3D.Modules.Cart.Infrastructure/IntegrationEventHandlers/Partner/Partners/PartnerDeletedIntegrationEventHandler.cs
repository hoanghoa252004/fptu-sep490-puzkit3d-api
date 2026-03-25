using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.Partners;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.Partners;

internal sealed class PartnerDeletedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerDeletedIntegrationEvent>
{
    private readonly CartDbContext _context;
    private readonly ICartUnitOfWork _unitOfWork;

    public PartnerDeletedIntegrationEventHandler(CartDbContext context, ICartUnitOfWork unitOfWork)
    {

        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        PartnerDeletedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ExecuteAsync(async () =>
        {
            var record = await _context.PartnerProductReplicas
            .Where(p => p.PartnerId == @event.PartnerId)
            .ExecuteUpdateAsync(x => x
            .SetProperty(p => p.IsActive, false)
            .SetProperty(p => p.UpdatedAt, @event.UpdatedAt),
            cancellationToken);

            return Result.Success();
        }, cancellationToken);
    }
}
