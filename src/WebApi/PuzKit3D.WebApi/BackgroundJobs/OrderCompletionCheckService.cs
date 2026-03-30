using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class OrderCompletionCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderCompletionCheckService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public OrderCompletionCheckService(
        IServiceProvider serviceProvider,
        ILogger<OrderCompletionCheckService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OrderCompletionCheckService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckOrderCompletion(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OrderCompletionCheckService");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("OrderCompletionCheckService stopped");
    }

    private async Task CheckOrderCompletion(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var inStockDbContext = scope.ServiceProvider.GetRequiredService<InStockDbContext>();
        var supportTicketDbContext = scope.ServiceProvider.GetRequiredService<SupportTicketDbContext>();
        var inStockUnitOfWork = scope.ServiceProvider.GetRequiredService<IInStockUnitOfWork>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        try
        {
            // Get all orders with status = HandedOverToDelivery and MustCompleteBefore is not null
            var ordersToCheck = inStockDbContext.InstockOrders
                .Where(o => o.Status == InstockOrderStatus.HandedOverToDelivery 
                    && o.MustCompleteBefore != null)
                .ToList();

            if (!ordersToCheck.Any())
                return;

            var now = DateTime.UtcNow;
            int completedCount = 0;
            var completedOrderIds = new List<(Guid OrderId, string Code, Guid CustomerId)>();

            foreach (var order in ordersToCheck)
            {
                // Check if order has expired
                if (order.MustCompleteBefore < now)
                {
                    // Check if there are unresolved support tickets for this order
                    var unresolvedTickets = supportTicketDbContext.SupportTickets
                        .Where(st => st.OrderId == order.Id.Value 
                            && st.Status != SupportTicketStatus.Resolved)
                        .ToList();

                    // If there are unresolved tickets, skip this order
                    if (unresolvedTickets.Any())
                    {
                        _logger.LogInformation("Order {OrderId} has unresolved support tickets, skipping auto-completion", order.Id);
                        continue;
                    }

                    // Update order status to Completed
                    var updateResult = order.UpdateStatus(InstockOrderStatus.Completed);
                    if (updateResult.IsFailure)
                    {
                        _logger.LogWarning("Failed to update order {OrderId} status to Completed", order.Id);
                        continue;
                    }

                    completedCount++;
                    completedOrderIds.Add((order.Id.Value, order.Code, order.CustomerId));
                }
            }

            // Save all changes and publish events
            if (completedCount > 0)
            {
                await inStockUnitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("OrderCompletionCheckService: {CompletedCount} orders marked as completed", completedCount);

                // Publish events
                foreach (var (orderId, code, customerId) in completedOrderIds)
                {
                    var statusChangedEvent = new InstockOrderStatusChangedIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        orderId,
                        code,
                        customerId,
                        InstockOrderStatus.Completed.ToString(),
                        DateTime.UtcNow);

                    await eventBus.PublishAsync(statusChangedEvent, cancellationToken);
                    _logger.LogInformation("Published InstockOrderStatusChangedIntegrationEvent for order {OrderId}", orderId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in OrderCompletionCheckService.CheckOrderCompletion");
            throw;
        }
    }
}

