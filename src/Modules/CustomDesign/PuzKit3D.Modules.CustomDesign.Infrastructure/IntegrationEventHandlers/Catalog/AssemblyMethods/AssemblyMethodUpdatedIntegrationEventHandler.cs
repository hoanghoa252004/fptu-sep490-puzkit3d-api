using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;

internal sealed class AssemblyMethodUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<AssemblyMethodUpdatedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public AssemblyMethodUpdatedIntegrationEventHandler(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        AssemblyMethodUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.AssemblyMethodReplicas
            .FirstOrDefaultAsync(a => a.Id == @event.AssemblyMethodId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        replica.Update(
            @event.Name,
            @event.Slug,
            @event.Description,
            replica.IsActive, // Keep existing IsActive value
            @event.UpdatedAt);

        _context.AssemblyMethodReplicas.Update(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
