using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;

internal sealed class AssemblyMethodCreatedIntegrationEventHandler
    : IIntegrationEventHandler<AssemblyMethodCreatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public AssemblyMethodCreatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        AssemblyMethodCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingReplica = await _context.AssemblyMethodReplicas
            .FirstOrDefaultAsync(a => a.Id == @event.AssemblyMethodId, cancellationToken);

        if (existingReplica is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = AssemblyMethodReplica.Create(
            @event.AssemblyMethodId,
            @event.Name,
            @event.Slug,
            @event.FactorPercentage,
            @event.Description,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.AssemblyMethodReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
