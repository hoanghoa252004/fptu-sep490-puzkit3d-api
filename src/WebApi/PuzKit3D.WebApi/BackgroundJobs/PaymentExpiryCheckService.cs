using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class PaymentExpiryCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentExpiryCheckService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(5);

    public PaymentExpiryCheckService(
        IServiceProvider serviceProvider,
        ILogger<PaymentExpiryCheckService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentExpiryCheckService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiredPayments(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PaymentExpiryCheckService");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("PaymentExpiryCheckService stopped");
    }

    private async Task CheckExpiredPayments(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var paymentDbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        var paymentUnitOfWork = scope.ServiceProvider.GetRequiredService<IPaymentUnitOfWork>();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        try
        {
            // Get all pending payments
            var pendingPayments = paymentDbContext.Payments
                .Where(p => p.Status == PaymentStatus.Pending)
                .ToList();

            if (!pendingPayments.Any())
                return;

            var now = DateTime.UtcNow;
            int expiredCount = 0;
            var orderIds = new List<Guid>();

            foreach (var payment in pendingPayments)
            {
                // Check if payment has expired
                if (payment.ExpiredAt < now)
                {
                    // Update payment status to Expired
                    var updatePaymentResult = payment.UpdateStatus(PaymentStatus.Expired);
                    if (updatePaymentResult.IsFailure)
                    {
                        _logger.LogWarning("Failed to update payment {PaymentId} status to Expired", payment.Id);
                        continue;
                    }

                    expiredCount++;
                    orderIds.Add(payment.ReferenceOrderId);
                }
            }

            // Save all changes and publish events
            if (expiredCount > 0)
            {
                await paymentUnitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("PaymentExpiryCheckService: {ExpiredCount} payments marked as expired", expiredCount);

                // Publish events to notify InStock module to update orders
                foreach (var orderId in orderIds)
                {
                    var orderExpiredEvent = new OrderExpiredToDoPaymentIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        orderId);

                    await eventBus.PublishAsync(orderExpiredEvent, cancellationToken);
                    _logger.LogInformation("Published OrderExpiredToDoPaymentIntegrationEvent for order {OrderId}", orderId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in PaymentExpiryCheckService.CheckExpiredPayments");
            throw;
        }
    }
}
