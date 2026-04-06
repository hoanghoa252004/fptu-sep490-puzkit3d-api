using Google.Cloud.AIPlatform.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.Modules.Media.Application.Services;
using PuzKit3D.SharedKernel.Application.Media;
using static Google.Rpc.Context.AttributeContext.Types;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class CustomDesignGenerationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CustomDesignGenerationService> _logger;
    private readonly IMediaService _mediaService;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);
    private readonly string _projectId = "friendly-aurora-492502-b4";
    private readonly string _location = "us-central1";
    private readonly PredictionServiceClient _predictionClient;

    public CustomDesignGenerationService(
        IServiceProvider serviceProvider,
        ILogger<CustomDesignGenerationService> logger,
        IMediaService mediaService,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mediaService = mediaService;        
        _predictionClient = PredictionServiceClient.Create();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CustomDesignGenerationService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessCustomDesignAssetsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CustomDesignGenerationService");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("CustomDesignGenerationService stopped");
    }


    private async Task ProcessCustomDesignAssetsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CustomDesignDbContext>();
        var mediaAssetService = scope.ServiceProvider.GetRequiredService<IMediaAssetService>();
        var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

        try
        {
            // Get all assets with ImageProcessing status (version 0)
            var assets = await dbContext.CustomDesignAssets
                .Where(a => a.Status == CustomDesignAssetStatus.ImageProcessing && a.Version == 0)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!assets.Any())
                return;

            int processedCount = 0;

            foreach (var asset in assets)
            {
                // Get the associated request
                var request = await dbContext.CustomDesignRequests
                    .FirstOrDefaultAsync(r => r.Id == asset.CustomDesignRequestId, cancellationToken);

                if (request == null)
                {
                    _logger.LogWarning("Request not found for asset {AssetId}", asset.Id.Value);
                    continue;
                }

                try
                {
                    // Process based on request type
                    if (request.Type == CustomDesignRequestType.Idea)
                    {
                        // For Idea type: use CustomerPrompt
                        if (string.IsNullOrWhiteSpace(request.CustomerPrompt))
                        {
                            _logger.LogWarning("CustomerPrompt is empty for asset {AssetId}", asset.Id.Value);
                            continue;
                        }

                        await CallGeminiApiAsync(
                            httpClient,
                            request.CustomerPrompt,
                            imageBase64Data: null,
                            asset.Id.Value,
                            cancellationToken);

                        processedCount++;
                    }
                    else if (request.Type == CustomDesignRequestType.Sketch)
                    {
                        // For Sketch type: use Sketches (download from S3 and encode to base64)
                        if (string.IsNullOrWhiteSpace(request.Sketches))
                        {
                            _logger.LogWarning("Sketches is empty for asset {AssetId}", asset.Id.Value);
                            continue;
                        }

                        // Get URLs from sketches paths
                        var sketchUrls = mediaAssetService.BuildAssetUrls(request.Sketches);

                        if (!sketchUrls.Any())
                        {
                            _logger.LogWarning("No sketch URLs found for asset {AssetId}", asset.Id.Value);
                            continue;
                        }

                        // Download first sketch and convert to base64
                        var imageBase64 = await DownloadAndEncodeImageAsync(sketchUrls[0], httpClient, cancellationToken);

                        if (string.IsNullOrWhiteSpace(imageBase64))
                        {
                            _logger.LogWarning("Failed to download/encode sketch for asset {AssetId}", asset.Id.Value);
                            continue;
                        }

                        // Determine prompt - could be customization of sketch or default
                        var prompt = !string.IsNullOrWhiteSpace(request.CustomerPrompt)
                            ? request.CustomerPrompt
                            : "Generate a multiview composite image based on this sketch";

                        await CallGeminiApiAsync(
                            httpClient,
                            prompt,
                            imageBase64,
                            asset.Id.Value,
                            cancellationToken);

                        processedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing asset {AssetId}", asset.Id.Value);
                }
            }

            if (processedCount > 0)
            {
                _logger.LogInformation("CustomDesignGenerationService: Processed {ProcessedCount} assets", processedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in CustomDesignGenerationService.ProcessCustomDesignAssetsAsync");
            throw;
        }
    }

    private async Task<string?> DownloadAndEncodeImageAsync(
        string imageUrl,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync(imageUrl, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to download image from {Url}. Status: {StatusCode}", imageUrl, response.StatusCode);
                return null;
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading/encoding image from {Url}", imageUrl);
            return null;
        }
    }

    private async Task CallGeminiApiAsync(
        HttpClient httpClient,
        string prompt,
        string? imageBase64Data,
        Guid assetId,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Calling Vertex AI Imagen for asset {AssetId}", assetId);

            // 1. Dùng đúng Model sinh ảnh (Imagen) và đúng định dạng Endpoint cho built-in model
            string modelId = "imagen-3.0-generate-001";
            string endpointPath = $"projects/{_projectId}/locations/{_location}/publishers/google/models/{modelId}";

            // 2. Build JSON Body bằng Struct của Protobuf
            var instance = new Google.Protobuf.WellKnownTypes.Struct();
            string basePrompt = prompt; // Ví dụ: "iphone 16 pokemon"
            string normalizedPrompt = $@"A professional 3D turnaround character sheet of {basePrompt}. " +
                          "The image MUST show exactly 4 distinct views arranged perfectly side-by-side horizontally: " +
                          "Front view, Right side view, Back view, Left side view. " +
                          "Orthographic projection, no perspective. " +
                          "Solid white background. " +
                          "All four views must perfectly match in scale, height, and design details.";
            instance.Fields.Add("prompt", Google.Protobuf.WellKnownTypes.Value.ForString(normalizedPrompt));

            // Nếu có ảnh sketch, đưa vào trường 'image'
            //if (!string.IsNullOrWhiteSpace(imageBase64Data))
            //{
            //    var imageStruct = new Google.Protobuf.WellKnownTypes.Struct();
            //    imageStruct.Fields.Add("bytesBase64Encoded", Google.Protobuf.WellKnownTypes.Value.ForString(imageBase64Data));
            //    instance.Fields.Add("image", Google.Protobuf.WellKnownTypes.Value.ForStruct(imageStruct));
            //}

            var instances = new List<Google.Protobuf.WellKnownTypes.Value>
        {
            Google.Protobuf.WellKnownTypes.Value.ForStruct(instance)
        };

            // Các thông số phụ (có thể bỏ qua)
            var parameters = new Google.Protobuf.WellKnownTypes.Struct();
            // parameters.Fields.Add("sampleCount", Google.Protobuf.WellKnownTypes.Value.ForNumber(1)); 
            parameters.Fields.Add("sampleCount", Google.Protobuf.WellKnownTypes.Value.ForNumber(1));
            // Ép ra khung hình ngang để dàn hàng 4 góc
            parameters.Fields.Add("aspectRatio", Google.Protobuf.WellKnownTypes.Value.ForString("16:9"));
            // 3. Gọi PredictAsync (dành cho Imagen) thay vì GenerateContentAsync
            var response = await _predictionClient.PredictAsync(
                endpoint: endpointPath,
                instances: instances,
                parameters: Google.Protobuf.WellKnownTypes.Value.ForStruct(parameters), // <-- Sửa đúng dòng này
                callSettings: null);

            // 4. Bóc tách ảnh Base64 từ kết quả trả về của Imagen
            string? generatedImageBase64 = null;
            if (response.Predictions.Count > 0)
            {
                var prediction = response.Predictions[0].StructValue;
                if (prediction.Fields.TryGetValue("bytesBase64Encoded", out var base64Value))
                {
                    generatedImageBase64 = base64Value.StringValue;
                }
            }

            if (string.IsNullOrWhiteSpace(generatedImageBase64))
            {
                _logger.LogWarning("No generated image found in response for asset {AssetId}", assetId);
                return;
            }

            _logger.LogInformation("Vertex AI successfully generated image for asset {AssetId}", assetId);

            // 5. Giữ nguyên logic Upload S3 cực chuẩn của bạn
            var s3Path = await UploadImageToS3Async(
                generatedImageBase64,
                assetId,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(s3Path))
            {
                _logger.LogError("Failed to upload generated image to S3 for asset {AssetId}", assetId);
                return;
            }

            // 6. Cập nhật DB
            await UpdateAssetWithGeneratedImageAsync(assetId, s3Path, cancellationToken);
            _logger.LogInformation("Asset {AssetId} successfully updated with generated image: {S3Path}", assetId, s3Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Vertex AI Imagen for asset {AssetId}", assetId);
        }
    }

    private string? ExtractGeneratedImageFromResponse(GenerateContentResponse response)
    {
        try
        {
            if (response?.Candidates == null || response.Candidates.Count == 0)
            {
                _logger.LogWarning("No candidates in response");
                return null;
            }

            var firstCandidate = response.Candidates[0];

            if (firstCandidate?.Content?.Parts == null || firstCandidate.Content.Parts.Count == 0)
            {
                _logger.LogWarning("No parts in first candidate");
                return null;
            }

            // Look for inline image data in the parts
            foreach (var part in firstCandidate.Content.Parts)
            {
                // Check if part has inline data (image bytes)
                if (part.InlineData != null && part.InlineData.Data != null && part.InlineData.Data.Length > 0)
                {
                    _logger.LogInformation("Found inline image data in response, converting to base64");
                    // Convert bytes to base64 string
                    var base64String = Convert.ToBase64String(part.InlineData.Data.ToByteArray());
                    return base64String;
                }

                // Fallback: check if part has text
                if (!string.IsNullOrWhiteSpace(part.Text))
                {
                    _logger.LogInformation("Found text in response (fallback)");
                    return part.Text;
                }
            }

            _logger.LogWarning("No image or text data found in response parts");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting generated image from response");
            return null;
        }
    }

    private async Task<string?> UploadImageToS3Async(
        string imageBase64,
        Guid assetId,
        CancellationToken cancellationToken)
    {
        try
        {
            // Convert base64 to bytes
            byte[] imageBytes;

            try
            {
                imageBytes = Convert.FromBase64String(imageBase64);
            }
            catch
            {
                // If it's not valid base64, try treating it as a URL and download it
                if (imageBase64.StartsWith("http"))
                {
                    using var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(imageBase64, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Failed to download image from URL: {Url}", imageBase64);
                        return null;
                    }

                    imageBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Invalid image data format for asset {AssetId}", assetId);
                    return null;
                }
            }

            // Generate S3 path
            var fileName = $"custom-design/multiview/{assetId:N}/composite-{DateTime.UtcNow:yyyyMMdd-HHmmss}.jpg";

            // Upload to S3 using IMediaService
            var uploadResult = await _mediaService.UploadFileAsync(
                imageBytes,
                fileName,
                "image/jpeg",
                cancellationToken);

            if (uploadResult.IsFailure)
            {
                _logger.LogError("Failed to upload generated image to S3 for asset {AssetId}: {Error}",
                    assetId, uploadResult.Error.Message);
                return null;
            }

            _logger.LogInformation("Image uploaded to S3: {FileName}", fileName);
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image to S3 for asset {AssetId}", assetId);
            return null;
        }
    }

    private async Task UpdateAssetWithGeneratedImageAsync(
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

            // Update asset with generated image path
            asset.Update(
                multiviewImages: asset.MultiviewImages,
                compositeMultiviewImage: s3Path,
                rough3DModel: asset.Rough3DModel,
                rough3DModelTaskId: asset.Rough3DModelTaskId,
                customerPrompt: asset.CustomerPrompt,
                normalizePrompt: asset.NormalizePrompt,
                isNeedSupport: asset.IsNeedSupport,
                isFinalDesign: asset.IsFinalDesign,
                updatedAt: DateTime.UtcNow);

            // Update status to RoughModelGenerating when composite image is generated
            asset.UpdateStatus(CustomDesignAssetStatus.RoughModelGenerating, DateTime.UtcNow);

            assetRepository.Update(asset);
            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Asset {AssetId} updated with composite image path: {S3Path} and status changed to RoughModelGenerating", assetId, s3Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating asset {AssetId} with generated image", assetId);
        }
    }
}
