using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.InStock.Application.Repositories;

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
        
        var instockOrderRepository = scope.ServiceProvider.GetRequiredService<IInstockOrderRepository>();
        
        // TODO: Implement order completion check logic
        // 1. Query orders with MustCompleteBefore < DateTime.UtcNow AND Status != Completed
        // 2. Update status to Completed
        // 3. Publish OrderAutoCompletedIntegrationEvent
        
        await Task.CompletedTask;
    }
}
