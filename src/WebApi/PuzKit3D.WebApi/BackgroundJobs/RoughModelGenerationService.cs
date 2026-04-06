using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.Modules.Media.Application.Services;
using PuzKit3D.SharedKernel.Application.Media;
using System.Text.Json;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class RoughModelGenerationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RoughModelGenerationService> _logger;
    private readonly IMediaAssetService _mediaAssetService;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);
    private readonly string _tripoApiKey = "tsk_qExBZgNUSOl7qxtt91yT7Fv3kSBR8HM6L09N0jA2Wh4";
    private readonly string _tripoApiEndpoint = "https://api.tripo3d.ai/v2/openapi/task";

    public RoughModelGenerationService(
        IServiceProvider serviceProvider,
        ILogger<RoughModelGenerationService> logger,
        IMediaAssetService mediaAssetService,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mediaAssetService = mediaAssetService;
        //_tripoApiKey = configuration["Tripo:ApiKey"]
        //    ?? throw new InvalidOperationException("Tripo API Key is not configured. Set Tripo:ApiKey in configuration.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RoughModelGenerationService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAssetsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RoughModelGenerationService");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("RoughModelGenerationService stopped");
    }

    private async Task ProcessAssetsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CustomDesignDbContext>();
        var assetRepository = scope.ServiceProvider.GetRequiredService<ICustomDesignAssetRepository>();
        var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

        try
        {
            // Get all assets with RoughModelGenerating status (any version)
            var assets = await dbContext.CustomDesignAssets
                .Where(a => a.Status == CustomDesignAssetStatus.RoughModelGenerating)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!assets.Any())
                return;

            _logger.LogInformation("Processing {Count} assets with RoughModelGenerating status", assets.Count);

            foreach (var asset in assets)
            {
                try
                {
                    // If no task ID yet, create one
                    if (string.IsNullOrWhiteSpace(asset.Rough3DModelTaskId))
                    {
                        await CreateModelGenerationTaskAsync(
                            asset,
                            dbContext,
                            httpClient,
                            cancellationToken);
                    }
                    else
                    {
                        // Check task status and download if ready
                        await CheckAndDownloadModelAsync(
                            asset,
                            dbContext,
                            httpClient,
                            cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing asset {AssetId}", asset.Id.Value);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ProcessAssetsAsync");
        }
    }

    private async Task CreateModelGenerationTaskAsync(
        CustomDesignAsset asset,
        CustomDesignDbContext dbContext,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get multiview images - should have 4 views (front, left, back, right)
            if (string.IsNullOrWhiteSpace(asset.MultiviewImages))
            {
                _logger.LogWarning("MultiviewImages is empty for asset {AssetId}", asset.Id.Value);
                return;
            }

            _logger.LogInformation("Creating Tripo model task for asset {AssetId}", asset.Id.Value);

            // Parse multiview images into list of URLs
            var imageUrls = _mediaAssetService.BuildAssetUrls(asset.MultiviewImages);

            if (imageUrls.Count == 0)
            {
                _logger.LogWarning("No multiview image URLs found for asset {AssetId}", asset.Id.Value);
                return;
            }

            // Prepare files array for Tripo API - exactly 4 items [front, left, back, right]
            // If we have less than 4 images, pad with nulls
            var files = new object?[]
            {
                imageUrls.Count > 0 ? new { type = "jpg", url = imageUrls[0] } : null,  // front
                imageUrls.Count > 1 ? new { type = "jpg", url = imageUrls[1] } : null,  // left
                imageUrls.Count > 2 ? new { type = "jpg", url = imageUrls[2] } : null,  // back
                imageUrls.Count > 3 ? new { type = "jpg", url = imageUrls[3] } : null   // right
            };

            var requestBody = new
            {
                type = "multiview_to_model",
                model_version = "v2.5-20250123",
                files = files,
                texture = true,
                pbr = true
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _tripoApiEndpoint)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            request.Headers.Add("Authorization", $"Bearer {_tripoApiKey}");

            var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Tripo API error for asset {AssetId}: {StatusCode} - {Error}",
                    asset.Id.Value, response.StatusCode, errorContent);
                return;
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            
            try
            {
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("task_id", out var taskIdElement))
                {
                    var taskId = taskIdElement.GetString();

                    if (!string.IsNullOrWhiteSpace(taskId))
                    {
                        // Update asset with task ID - need to fetch fresh instance
                        var updateAsset = await dbContext.CustomDesignAssets
                            .FirstOrDefaultAsync(a => a.Id == asset.Id, cancellationToken);

                        if (updateAsset != null)
                        {
                            updateAsset.SetRough3DModelTaskId(taskId);
                            await dbContext.SaveChangesAsync(cancellationToken);
                            _logger.LogInformation("Created Tripo task {TaskId} for asset {AssetId} with {ImageCount} images",
                                taskId, asset.Id.Value, imageUrls.Count);
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Tripo API response for asset {AssetId}",
                    asset.Id.Value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating model generation task for asset {AssetId}",
                asset.Id.Value);
        }
    }

    private async Task CheckAndDownloadModelAsync(
        CustomDesignAsset asset,
        CustomDesignDbContext dbContext,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        try
        {
            var taskId = asset.Rough3DModelTaskId;

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_tripoApiEndpoint}/{taskId}");
            request.Headers.Add("Authorization", $"Bearer {_tripoApiKey}");

            var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to get task status for task {TaskId}: {StatusCode}", taskId, response.StatusCode);
                return;
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            try
            {
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("data", out var data))
                {
                    if (data.TryGetProperty("status", out var statusElement))
                    {
                        var status = statusElement.GetString();
                        _logger.LogInformation("Task {TaskId} status: {Status} for asset {AssetId}",
                            taskId, status, asset.Id.Value);

                        if (status == "success")
                        {
                            // Extract model URL from response
                            string? modelUrl = null;

                            // Try newer response format first (result.pbr_model.url)
                            if (data.TryGetProperty("result", out var result))
                            {
                                if (result.TryGetProperty("pbr_model", out var pbrModel))
                                {
                                    if (pbrModel.TryGetProperty("url", out var urlElement))
                                    {
                                        modelUrl = urlElement.GetString();
                                    }
                                }
                            }
                            // Fallback to older format (output.pbr_model as string)
                            else if (data.TryGetProperty("output", out var output))
                            {
                                if (output.TryGetProperty("pbr_model", out var pbrModelUrl))
                                {
                                    modelUrl = pbrModelUrl.GetString();
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(modelUrl))
                            {
                                _logger.LogInformation("Downloading model from {Url} for asset {AssetId}",
                                    modelUrl, asset.Id.Value);

                                // Download and upload model
                                var s3Path = await DownloadAndUploadModelAsync(
                                    modelUrl,
                                    asset.Id.Value,
                                    httpClient,
                                    cancellationToken);

                                if (!string.IsNullOrWhiteSpace(s3Path))
                                {
                                    // Update asset with model path and status
                                    await UpdateAssetWithModelAsync(
                                        asset.Id.Value,
                                        s3Path,
                                        cancellationToken);
                                }
                            }
                            else
                            {
                                _logger.LogWarning("No model URL found in successful response for task {TaskId}",
                                    taskId);
                            }
                        }
                        else if (status == "failed" || status == "banned" || status == "expired" || status == "cancelled")
                        {
                            _logger.LogError("Task {TaskId} failed with status {Status} for asset {AssetId}",
                                taskId, status, asset.Id.Value);
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse task status response for task {TaskId}",
                    taskId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking model status for asset {AssetId}",
                asset.Id.Value);
        }
    }

    private async Task<string?> DownloadAndUploadModelAsync(
        string modelUrl,
        Guid assetId,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync(modelUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to download model from {Url}. Status: {StatusCode}",
                    modelUrl, response.StatusCode);
                return null;
            }

            var modelBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var fileName = $"custom-design/3d-models/{assetId:N}/model-{DateTime.UtcNow:yyyyMMdd-HHmmss}.glb";

            using var scope = _serviceProvider.CreateScope();
            var mediaService = scope.ServiceProvider.GetRequiredService<IMediaService>();

            var uploadResult = await mediaService.UploadFileAsync(
                modelBytes,
                fileName,
                "model/gltf-binary",
                cancellationToken);

            if (uploadResult.IsFailure)
            {
                _logger.LogError("Failed to upload model to S3 for asset {AssetId}: {Error}",
                    assetId, uploadResult.Error.Message);
                return null;
            }

            _logger.LogInformation("Model uploaded to S3: {FileName}", fileName);
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading/uploading model for asset {AssetId}", assetId);
            return null;
        }
    }

    private async Task UpdateAssetWithModelAsync(
        Guid assetId,
        string s3Path,
        CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CustomDesignDbContext>();
            var assetRepository = scope.ServiceProvider.GetRequiredService<ICustomDesignAssetRepository>();

            var asset = await dbContext.CustomDesignAssets
                .FirstOrDefaultAsync(a => a.Id == CustomDesignAssetId.From(assetId), cancellationToken);

            if (asset == null)
            {
                _logger.LogWarning("Asset {AssetId} not found for update", assetId);
                return;
            }

            // Update asset with model path
            asset.Update(
                multiviewImages: asset.MultiviewImages,
                compositeMultiviewImage: asset.CompositeMultiviewImage,
                rough3DModel: s3Path,
                rough3DModelTaskId: asset.Rough3DModelTaskId,
                customerPrompt: asset.CustomerPrompt,
                normalizePrompt: asset.NormalizePrompt,
                isNeedSupport: asset.IsNeedSupport,
                isFinalDesign: asset.IsFinalDesign,
                updatedAt: DateTime.UtcNow);

            // Update status to Completed
            asset.UpdateStatus(CustomDesignAssetStatus.Completed, DateTime.UtcNow);

            assetRepository.Update(asset);
            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Asset {AssetId} completed with model: {S3Path}", assetId, s3Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating asset with model for asset {AssetId}", assetId);
        }
    }
}
