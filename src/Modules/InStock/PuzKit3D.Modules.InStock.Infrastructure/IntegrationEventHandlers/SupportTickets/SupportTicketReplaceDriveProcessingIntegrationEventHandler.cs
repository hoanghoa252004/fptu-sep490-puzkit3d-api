using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.SupportTickets;

internal sealed class SupportTicketReplaceDriveProcessingIntegrationEventHandler
    : IIntegrationEventHandler<SupportTicketReplaceDriveProcessingIntegrationEvent>
{
    private readonly IDriveReplicaRepository _driveReplicaRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public SupportTicketReplaceDriveProcessingIntegrationEventHandler(
        IDriveReplicaRepository driveReplicaRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _driveReplicaRepository = driveReplicaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        SupportTicketReplaceDriveProcessingIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var item in @event.Items)
            {
                // Get drive replica by drive ID
                var driveReplica = await _driveReplicaRepository.GetByIdAsync(item.DriveId, cancellationToken);

                if (driveReplica is null)
                {
                    // Drive replica not found, skip
                    continue;
                }

                // Reduce drive quantity for replacement
                int newQuantity = driveReplica.QuantityInStock - item.Quantity;
                if (newQuantity >= 0)
                {
                    driveReplica.Update(
                        driveReplica.Name,
                        driveReplica.Description,
                        driveReplica.MinVolume,
                        newQuantity,
                        driveReplica.IsActive,
                        DateTime.UtcNow);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // Log error or handle appropriately
            throw;
        }
    }
}

