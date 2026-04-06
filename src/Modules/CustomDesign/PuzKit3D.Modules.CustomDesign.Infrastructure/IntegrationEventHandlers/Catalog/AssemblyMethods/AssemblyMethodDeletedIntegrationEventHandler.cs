using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;

internal sealed class AssemblyMethodDeletedIntegrationEventHandler
    : IIntegrationEventHandler<AssemblyMethodDeletedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public AssemblyMethodDeletedIntegrationEventHandler(CustomDesignDbContext context)
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

        _context.AssemblyMethodReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
