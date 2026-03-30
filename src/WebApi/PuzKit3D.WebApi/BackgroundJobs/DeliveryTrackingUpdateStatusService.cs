using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Contract.Delivery;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Application.UnitOfWork;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class DeliveryTrackingUpdateStatusService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeliveryTrackingUpdateStatusService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public DeliveryTrackingUpdateStatusService(
        IServiceProvider serviceProvider,
        ILogger<DeliveryTrackingUpdateStatusService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeliveryTrackingUpdateStatusService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateDeliveryTrackingStatus(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeliveryTrackingUpdateStatusService");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("DeliveryTrackingUpdateStatusService stopped");
    }

    private async Task UpdateDeliveryTrackingStatus(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var deliveryDbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
        var deliveryTrackingRepository = scope.ServiceProvider.GetRequiredService<IDeliveryTrackingRepository>();
        var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
        var deliveryUnitOfWork = scope.ServiceProvider.GetRequiredService<IDeliveryUnitOfWork>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        try
        {
            // Get all delivery trackings with status not Delivered and not Returned
            var trackingsToUpdate = deliveryDbContext.DeliveryTrackings
                .Where(dt => dt.Status != DeliveryTrackingStatus.Delivered 
                    && dt.Status != DeliveryTrackingStatus.Returned)
                .ToList();

            if (!trackingsToUpdate.Any())
                return;

            int updatedCount = 0;
            var statusChangedTrackings = new List<(DeliveryTracking tracking, DeliveryTrackingStatus oldStatus)>();

            foreach (var tracking in trackingsToUpdate)
            {
                var oldStatus = tracking.Status;
                var updateResult = await SyncDeliveryTrackingStatusFromGhnAsync(
                    tracking,
                    deliveryService,
                    cancellationToken);

                if (updateResult && tracking.Status != oldStatus)
                {
                    updatedCount++;
                    statusChangedTrackings.Add((tracking, oldStatus));
                }
            }

            // Save all changes
            if (updatedCount > 0)
            {
                await deliveryUnitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("DeliveryTrackingUpdateStatusService: {UpdatedCount} delivery trackings updated", updatedCount);

                // Publish events for completed deliveries
                foreach (var (tracking, oldStatus) in statusChangedTrackings)
                {
                    if (tracking.Status == DeliveryTrackingStatus.Delivered)
                    {
                        await eventBus.PublishAsync(
                            new OrderDeliveredIntegrationEvent(
                                Guid.NewGuid(),
                                DateTime.UtcNow,
                                tracking.OrderId),
                            cancellationToken);
                        _logger.LogInformation("Published OrderDeliveredIntegrationEvent for order {OrderId}", tracking.OrderId);
                    }
                    else if (tracking.Status == DeliveryTrackingStatus.Returned)
                    {
                        await eventBus.PublishAsync(
                            new OrderReturnedIntegrationEvent(
                                Guid.NewGuid(),
                                DateTime.UtcNow,
                                tracking.OrderId),
                            cancellationToken);
                        _logger.LogInformation("Published OrderReturnedIntegrationEvent for order {OrderId}", tracking.OrderId);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in DeliveryTrackingUpdateStatusService.UpdateDeliveryTrackingStatus");
            throw;
        }
    }

    private async Task<bool> SyncDeliveryTrackingStatusFromGhnAsync(
        DeliveryTracking tracking,
        IDeliveryService deliveryService,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(tracking.DeliveryOrderCode))
            return false;

        var result = await deliveryService.GetShippingOrderDetailAsync(tracking.DeliveryOrderCode);

        if (result.IsFailure)
            return false;

        try
        {
            // Serialize the response to JSON and parse it
            var json = JsonSerializer.Serialize(result.Value);
            using var doc = JsonDocument.Parse(json);

            // Navigate to data.status
            if (!doc.RootElement.TryGetProperty("data", out var dataProperty))
                return false;

            if (!dataProperty.TryGetProperty("status", out var statusProperty))
                return false;

            var ghnStatus = statusProperty.GetString();

            // Map GHN status to DeliveryTrackingStatus
            var mappedStatus = MapGhnStatusToDeliveryTrackingStatus(ghnStatus);

            // Check if status needs to be updated
            if (mappedStatus.HasValue && mappedStatus.Value != tracking.Status)
            {
                // Update the tracking status based on the new status
                var updateResult = mappedStatus.Value switch
                {
                    DeliveryTrackingStatus.Picked => tracking.MarkAsPicked(),
                    DeliveryTrackingStatus.Delivering => tracking.MarkAsShipping(),
                    DeliveryTrackingStatus.Delivered => tracking.MarkAsDelivered(),
                    DeliveryTrackingStatus.Return => tracking.MarkAsReturn(),
                    DeliveryTrackingStatus.Returned => tracking.MarkAsReturned(),
                    _ => Result.Failure(Error.Validation("InvalidStatus", "Invalid delivery tracking status"))
                };

                return updateResult.IsSuccess;
            }

            return false;
        }
        catch
        {
            // Silently continue if parsing fails
            return false;
        }
    }

    private static DeliveryTrackingStatus? MapGhnStatusToDeliveryTrackingStatus(string? ghnStatus)
    {
        if (string.IsNullOrWhiteSpace(ghnStatus))
            return null;

        return ghnStatus.ToLowerInvariant() switch
        {
            "picked" => DeliveryTrackingStatus.Picked,
            "delivering" => DeliveryTrackingStatus.Delivering,
            "delivered" => DeliveryTrackingStatus.Delivered,
            "return" => DeliveryTrackingStatus.Return,
            "returned" => DeliveryTrackingStatus.Returned,
            _ => null
        };
    }
}
