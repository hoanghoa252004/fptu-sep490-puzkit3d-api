using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Materials;

internal sealed class MaterialDeletedIntegrationEventHandler
    : IIntegrationEventHandler<MaterialDeletedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public MaterialDeletedIntegrationEventHandler(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        MaterialDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.MaterialReplicas
            .FirstOrDefaultAsync(m => m.Id == @event.MaterialId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        _context.MaterialReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
