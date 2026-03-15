using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;

internal sealed class AssemblyMethodDeletedIntegrationEventHandler
    : IIntegrationEventHandler<AssemblyMethodDeletedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public AssemblyMethodDeletedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        AssemblyMethodDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.AssemblyMethodReplicas
            .FirstOrDefaultAsync(a => a.Id == @event.AssemblyMethodId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        // Check if any InstockProduct is using this AssemblyMethod
        var hasProducts = await _context.InstockProducts
            .AnyAsync(p => p.AssemblyMethodId == @event.AssemblyMethodId, cancellationToken);

        if (hasProducts)
        {
            throw new PuzKit3DException("This assembly method can be delete because there is one or more products belongs");
        }

        _context.AssemblyMethodReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

