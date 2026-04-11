using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Materials;

internal sealed class MaterialUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<MaterialUpdatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public MaterialUpdatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        MaterialUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.MaterialReplicas
            .FirstOrDefaultAsync(m => m.Id == @event.MaterialId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        replica.Update(
            @event.Name,
            @event.Slug,
            @event.FactorPercentage,
            @event.BasePrice,
            @event.Description,
            @event.IsActive,
            @event.UpdatedAt);

        _context.MaterialReplicas.Update(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
