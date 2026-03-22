using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Materials;

internal sealed class MaterialCreatedIntegrationEventHandler
    : IIntegrationEventHandler<MaterialCreatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public MaterialCreatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        MaterialCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingReplica = await _context.MaterialReplicas
            .FirstOrDefaultAsync(m => m.Id == @event.MaterialId, cancellationToken);

        if (existingReplica is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = MaterialReplica.Create(
            @event.MaterialId,
            @event.Name,
            @event.Slug,
            @event.Description,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.MaterialReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
