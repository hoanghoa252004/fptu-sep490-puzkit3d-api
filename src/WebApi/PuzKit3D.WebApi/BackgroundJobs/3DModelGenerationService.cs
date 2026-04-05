using PuzKit3D.SharedKernel.Application._3DModel;
using PuzKit3D.SharedKernel.Application.Image;
using PuzKit3D.SharedKernel.Application.Queue;

namespace PuzKit3D.WebApi.BackgroundJobs;

public class _3DModelGenerationService : BackgroundService
{
    private readonly IJobQueue _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<_3DModelGenerationService> _logger;

    public _3DModelGenerationService(
        IJobQueue queue,
        IServiceProvider serviceProvider,
        ILogger<_3DModelGenerationService> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var taskId = await _queue.DequeueAsync(stoppingToken);

                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<I3DModelGenerationService>();

                await service.ProcessAsync(taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing 3D model task");
            }
        }
    }
}
