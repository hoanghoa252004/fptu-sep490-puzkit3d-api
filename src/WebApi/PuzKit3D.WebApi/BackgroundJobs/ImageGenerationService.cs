using PuzKit3D.SharedKernel.Application.Image;
using PuzKit3D.SharedKernel.Application.Queue;

namespace PuzKit3D.WebApi.BackgroundJobs;

public class ImageGenerationService : BackgroundService
{
    private readonly IJobQueue _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ImageGenerationService> _logger;

    public ImageGenerationService(
        IJobQueue queue,
        IServiceProvider serviceProvider,
        ILogger<ImageGenerationService> logger)
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
                var service = scope.ServiceProvider.GetRequiredService<IImageGenerationService>();

                await service.ProcessAsync(taskId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing image task");
            }
        }
    }
}