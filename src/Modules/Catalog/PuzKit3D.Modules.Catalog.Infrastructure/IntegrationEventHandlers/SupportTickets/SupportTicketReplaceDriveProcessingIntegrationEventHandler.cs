using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.Modules.Catalog.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketReplaceDriveProcessingIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketReplaceDriveProcessingIntegrationEvent>
{
    private readonly IDriveRepository _driveRepository;
    private readonly CatalogDbContext _dbContext;

    public SupportTicketReplaceDriveProcessingIntegrationEventHandler(
        IDriveRepository driveRepository,
        CatalogDbContext dbContext)
    {
        _driveRepository = driveRepository;
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        SupportTicketReplaceDriveProcessingIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in @event.Items)
            {
                // Get drive by ID
                var drive = await _driveRepository.GetByIdAsync(
                    DriveId.From(item.DriveId),
                    cancellationToken);

                if (drive is null)
                {
                    // Drive not found, skip
                    continue;
                }

                // Reduce drive quantity for replacement
                var result = drive.ReduceQuantity(item.Quantity);
                if (result.IsSuccess)
                {
                    _dbContext.Update(drive);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Log error or handle appropriately
            throw;
        }
    }
}
