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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static Google.Rpc.Context.AttributeContext.Types;

namespace PuzKit3D.WebApi.BackgroundJobs;

public sealed class CustomDesignGenerationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CustomDesignGenerationService> _logger;
    private readonly IMediaService _mediaService;
    private readonly IMediaAssetService _mediaAssetService;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);
    private readonly string _projectId = "friendly-aurora-492502-b4";
    private readonly string _location = "us-central1";
    private readonly PredictionServiceClient _predictionClient;

    public CustomDesignGenerationService(
        IServiceProvider serviceProvider,
        ILogger<CustomDesignGenerationService> logger,
        IMediaService mediaService,
        IConfiguration configuration,
        IMediaAssetService mediaAssetService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _mediaService = mediaService;        
        _predictionClient = PredictionServiceClient.Create();
        _mediaAssetService = mediaAssetService;
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
            // Get all assets with ImageProcessing status (any version)
            var assets = await dbContext.CustomDesignAssets
                .Where(a => a.Status == CustomDesignAssetStatus.ImageProcessing)
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
                    string? imageBase64Data = null;
                    string prompt = "";

                    if (asset.Version == 0)
                    {
                        // Version 0: Use original request data
                        if (request.Type == CustomDesignRequestType.Idea)
                        {
                            // For Idea type: use and normalize CustomerPrompt from request
                            if (string.IsNullOrWhiteSpace(asset.CustomerPrompt))
                            {
                                _logger.LogWarning("CustomerPrompt is empty for asset {AssetId}", asset.Id.Value);
                                continue;
                            }

                            // Normalize the prompt
                            prompt = NormalizePrompt(asset.CustomerPrompt!);
                            imageBase64Data = null;
                        }
                        else if (request.Type == CustomDesignRequestType.Sketch)
                        {
                            // For Sketch type: use Sketches from request (comma-separated URIs)
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

                            // Download all sketches and convert to base64
                            var sketchBase64List = new List<string>();
                            foreach (var sketchUrl in sketchUrls)
                            {
                                var base64 = await DownloadAndEncodeImageAsync(sketchUrl, httpClient, cancellationToken);
                                if (!string.IsNullOrWhiteSpace(base64))
                                {
                                    sketchBase64List.Add(base64);
                                }
                            }

                            if (!sketchBase64List.Any())
                            {
                                _logger.LogWarning("Failed to download/encode any sketches for asset {AssetId}", asset.Id.Value);
                                continue;
                            }

                            // For now, use the first sketch (can be extended to handle multiple)
                            // TODO: Support multiple sketch inputs to Gemini
                            imageBase64Data = sketchBase64List[0];

                            // Build prompt with customer customization if provided
                            if (!string.IsNullOrWhiteSpace(asset.CustomerPrompt))
                            {
                                prompt = NormalizePrompt(asset.CustomerPrompt);
                            }
                            else
                            {
                                prompt = "Generate a multiview composite image showing front, back, right, and left views based on these sketches";
                            }
                        }
                    }
                    else
                    {
                        // Version > 0: Use CompositeMultiviewImage from previous version
                        var previousAsset = await dbContext.CustomDesignAssets
                            .Where(a => a.CustomDesignRequestId == asset.CustomDesignRequestId && a.Version == asset.Version - 1)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (previousAsset == null || string.IsNullOrWhiteSpace(previousAsset.CompositeMultiviewImage))
                        {
                            _logger.LogWarning("Previous version asset not found or no composite image for asset {AssetId} version {Version}", 
                                asset.Id.Value, asset.Version);
                            continue;
                        }

                        // Get the previous composite image and use it as input
                        var previousImageUrl = mediaAssetService.BuildAssetUrl(previousAsset.CompositeMultiviewImage);
                        imageBase64Data = await DownloadAndEncodeImageAsync(previousImageUrl, httpClient, cancellationToken);

                        if (string.IsNullOrWhiteSpace(imageBase64Data))
                        {
                            _logger.LogWarning("Failed to download previous composite image for asset {AssetId}", asset.Id.Value);
                            continue;
                        }

                        // Use request customer prompt or default refinement prompt
                        if (!string.IsNullOrWhiteSpace(asset.CustomerPrompt))
                        {
                            prompt = NormalizePrompt(asset.CustomerPrompt);
                        }
                        else
                        {
                            prompt = "Refine and improve this composite image with better details and quality";
                        }
                    }

                    // Call Gemini API with prepared data
                    await CallGeminiApiAsync(
                        httpClient,
                        prompt,
                        imageBase64Data,
                        asset.Id.Value,
                        prompt, // Pass normalized prompt to save it
                        cancellationToken);

                    processedCount++;
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
    string normalizedPrompt,
    CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Calling Vertex AI Nano Banana 2 (Gemini Flash Image) for asset {AssetId}", assetId);

            // 1. Dùng Endpoint của Nano Banana 2
            //string modelId = "gemini-3.1-flash-image"; // Hoặc gemini-3-flash-image tùy region
            string modelId = "gemini-3-pro-image-preview";
            string endpointPath = $"projects/{_projectId}/locations/{_location}/publishers/google/models/{modelId}";

            bool isImageToImage = !string.IsNullOrWhiteSpace(imageBase64Data);

            // 2. Chuẩn bị Prompt
            string finalPrompt;
            if (isImageToImage)
            {
                finalPrompt = $"Based on this sketch, {normalizedPrompt}. Render it as a high-quality 3D model turnaround.";
                _logger.LogInformation("Using Image-to-Image (Sketch) mode for asset {AssetId}", assetId);
            }
            else
            {
                finalPrompt = $@"A professional 3D turnaround character sheet of {prompt}. " +
                              "The image MUST show exactly 4 distinct views arranged perfectly side-by-side horizontally: " +
                              "Front view, Right side view, Back view, Left side view. " +
                              "Orthographic projection, no perspective. Solid white background. " +
                              "All four views must perfectly match in scale, height, and design details.";
                _logger.LogInformation("Using Text-to-Image (Idea) mode for asset {AssetId}", assetId);
            }

            // 3. Xây dựng đối tượng Content (Chuẩn cấu trúc của Gemini)
            var content = new Content { Role = "user" };
            content.Parts.Add(new Part { Text = finalPrompt });

            // Nếu có ảnh Sketch thì nhét vào làm mồi cho AI
            if (isImageToImage)
            {
                try
                {
                    var imageBytes = Convert.FromBase64String(imageBase64Data);
                    content.Parts.Add(new Part
                    {
                        InlineData = new Blob
                        {
                            MimeType = "image/jpeg", // Hoặc image/png
                            Data = Google.Protobuf.ByteString.CopyFrom(imageBytes)
                        }
                    });
                }
                catch
                {
                    _logger.LogError("Failed to convert base64 image data for asset {AssetId}", assetId);
                    return;
                }
            }

            // 4. Khởi tạo Request
            var generateContentRequest = new GenerateContentRequest
            {
                Model = endpointPath
            };
            generateContentRequest.Contents.Add(content);

            // 5. ÉP GEMINI TRẢ VỀ HÌNH ẢNH (Bí kíp nằm ở đây!)
            generateContentRequest.GenerationConfig = new GenerationConfig
            {
                Temperature = 0.5f // Nhiệt độ thấp một chút để bám sát sketch
            };

            // Thêm ResponseModalities để báo Vertex AI rằng: "Đừng trả Text, hãy trả mảng byte hình ảnh!"
            generateContentRequest.GenerationConfig.ResponseModalities.Add(GenerationConfig.Types.Modality.Image);

            // 6. Gọi API (Dùng GenerateContentAsync thay vì PredictAsync)
            var response = await _predictionClient.GenerateContentAsync(generateContentRequest, cancellationToken: cancellationToken);

            // 7. Gọi lại hàm bóc tách Base64 của bạn (ExtractGeneratedImageFromResponse)
            var generatedImageBase64 = ExtractGeneratedImageFromResponse(response);

            if (string.IsNullOrWhiteSpace(generatedImageBase64))
            {
                _logger.LogWarning("No generated image found in response for asset {AssetId}", assetId);
                return;
            }

            _logger.LogInformation("Vertex AI successfully generated image for asset {AssetId}", assetId);

            // 8. Upload lên S3 và cập nhật DB (Logic cũ của bạn chạy rất mượt)
            var s3Path = await UploadImageToS3Async(generatedImageBase64, assetId, cancellationToken);

            if (string.IsNullOrWhiteSpace(s3Path))
            {
                _logger.LogError("Failed to upload generated image to S3 for asset {AssetId}", assetId);
                return;
            }

            await UpdateAssetWithGeneratedImageAsync(assetId, s3Path, finalPrompt, cancellationToken);
        }
        catch (Grpc.Core.RpcException grpcEx)
        {
            _logger.LogError(grpcEx, "Vertex AI gRPC Error for asset {AssetId}: {Status} - {Detail}", assetId, grpcEx.Status.StatusCode, grpcEx.Status.Detail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling Vertex AI Gemini for asset {AssetId}", assetId);
        }
    }

    //private async Task CallGeminiApiAsync(
    //HttpClient httpClient,
    //string prompt,
    //string? imageBase64Data,
    //Guid assetId,
    //string normalizedPrompt,
    //CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        _logger.LogInformation("Calling Vertex AI Imagen for asset {AssetId}", assetId);

    //        // 1. Endpoint của Imagen 3
    //        string modelId = "imagen-3.0-generate-001";
    //        string endpointPath = $"projects/{_projectId}/locations/{_location}/publishers/google/models/{modelId}";

    //        // 2. Build JSON Body bằng Struct của Protobuf
    //        var instance = new Google.Protobuf.WellKnownTypes.Struct();
    //        bool isImageToImage = !string.IsNullOrWhiteSpace(imageBase64Data);

    //        string finalPrompt;
    //        if (isImageToImage)
    //        {
    //            // Prompt cho Sketch-to-Image cần nhấn mạnh việc sử dụng ảnh gốc
    //            finalPrompt = $"Based on this sketch, {normalizedPrompt}. Render it as a high-quality 3D model turnaround.";
    //            _logger.LogInformation("Using Image-to-Image mode for asset {AssetId}", assetId);

    //            var imageStruct = new Google.Protobuf.WellKnownTypes.Struct();
    //            imageStruct.Fields.Add("bytesBase64Encoded", Google.Protobuf.WellKnownTypes.Value.ForString(imageBase64Data));
    //            instance.Fields.Add("image", Google.Protobuf.WellKnownTypes.Value.ForStruct(imageStruct));
    //        }
    //        else
    //        {
    //            finalPrompt = $@"A professional 3D turnaround character sheet of {prompt}. " +
    //                          "The image MUST show exactly 4 distinct views arranged perfectly side-by-side horizontally: " +
    //                          "Front view, Right side view, Back view, Left side view. " +
    //                          "Orthographic projection, no perspective. Solid white background. " +
    //                          "All four views must perfectly match in scale, height, and design details.";
    //            _logger.LogInformation("Using Text-to-Image mode for asset {AssetId}", assetId);
    //        }

    //        instance.Fields.Add("prompt", Google.Protobuf.WellKnownTypes.Value.ForString(finalPrompt));

    //        var instances = new List<Google.Protobuf.WellKnownTypes.Value>
    //    {
    //        Google.Protobuf.WellKnownTypes.Value.ForStruct(instance)
    //    };

    //        // 3. Cấu hình Parameters
    //        var parameters = new Google.Protobuf.WellKnownTypes.Struct();
    //        parameters.Fields.Add("sampleCount", Google.Protobuf.WellKnownTypes.Value.ForNumber(1));
    //        // Thêm personGeneration nếu nhân vật của bạn có yếu tố con người để tránh bị block bởi bộ lọc an toàn
    //        parameters.Fields.Add("personGeneration", Google.Protobuf.WellKnownTypes.Value.ForString("ALLOW_ADULT"));

    //        // CRITICAL FIX: Chỉ set aspectRatio khi KHÔNG truyền ảnh gốc. Nếu có ảnh gốc, API sẽ tự lấy tỷ lệ của ảnh gốc.
    //        if (!isImageToImage)
    //        {
    //            parameters.Fields.Add("aspectRatio", Google.Protobuf.WellKnownTypes.Value.ForString("16:9"));
    //        }

    //        // 4. Gọi PredictAsync
    //        var response = await _predictionClient.PredictAsync(
    //            endpoint: endpointPath,
    //            instances: instances,
    //            parameters: Google.Protobuf.WellKnownTypes.Value.ForStruct(parameters),
    //            callSettings: null);

    //        // 5. Bóc tách ảnh Base64
    //        string? generatedImageBase64 = null;
    //        if (response.Predictions.Count > 0)
    //        {
    //            var prediction = response.Predictions[0].StructValue;
    //            if (prediction.Fields.TryGetValue("bytesBase64Encoded", out var base64Value))
    //            {
    //                generatedImageBase64 = base64Value.StringValue;
    //            }
    //        }

    //        if (string.IsNullOrWhiteSpace(generatedImageBase64))
    //        {
    //            _logger.LogWarning("No generated image found in response for asset {AssetId}. Response: {Response}", assetId, response.ToString());
    //            return;
    //        }

    //        _logger.LogInformation("Vertex AI successfully generated image for asset {AssetId}", assetId);

    //        // 6. Upload S3
    //        var s3Path = await UploadImageToS3Async(generatedImageBase64, assetId, cancellationToken);

    //        if (string.IsNullOrWhiteSpace(s3Path))
    //        {
    //            _logger.LogError("Failed to upload generated image to S3 for asset {AssetId}", assetId);
    //            return;
    //        }

    //        // 7. Cập nhật DB
    //        await UpdateAssetWithGeneratedImageAsync(assetId, s3Path, finalPrompt, cancellationToken);
    //    }
    //    catch (Grpc.Core.RpcException grpcEx)
    //    {
    //        // Bắt lỗi cụ thể của gRPC (Vertex AI) để dễ debug (ví dụ: lỗi filter an toàn, lỗi quota)
    //        _logger.LogError(grpcEx, "Vertex AI gRPC Error for asset {AssetId}: {Status} - {Detail}", assetId, grpcEx.Status.StatusCode, grpcEx.Status.Detail);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Unexpected error calling Vertex AI Imagen for asset {AssetId}", assetId);
    //    }
    //}

    //private async Task CallGeminiApiAsync(
    //    HttpClient httpClient,
    //    string prompt,
    //    string? imageBase64Data,
    //    Guid assetId,
    //    string normalizedPrompt,
    //    CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        _logger.LogInformation("Calling Vertex AI Imagen for asset {AssetId}", assetId);

    //        // 1. Dùng đúng Model sinh ảnh (Imagen) và đúng định dạng Endpoint cho built-in model
    //        string modelId = "imagen-3.0-generate-001";
    //        string endpointPath = $"projects/{_projectId}/locations/{_location}/publishers/google/models/{modelId}";

    //        // 2. Build JSON Body bằng Struct của Protobuf
    //        var instance = new Google.Protobuf.WellKnownTypes.Struct();

    //        // Determine final prompt based on whether image is provided
    //        string finalPrompt;
    //        if (!string.IsNullOrWhiteSpace(imageBase64Data))
    //        {
    //            // When we have a sketch/image, use the normalized prompt which should mention refinement
    //            finalPrompt = normalizedPrompt;
    //            _logger.LogInformation("Using image-based prompt for asset {AssetId}", assetId);
    //        }
    //        else
    //        {
    //            // When no image, use a more detailed generation prompt
    //            finalPrompt = $@"A professional 3D turnaround character sheet of {prompt}. " +
    //                          "The image MUST show exactly 4 distinct views arranged perfectly side-by-side horizontally: " +
    //                          "Front view, Right side view, Back view, Left side view. " +
    //                          "Orthographic projection, no perspective. " +
    //                          "Solid white background. " +
    //                          "All four views must perfectly match in scale, height, and design details.";
    //            _logger.LogInformation("Using text-only prompt for asset {AssetId}", assetId);
    //        }

    //        instance.Fields.Add("prompt", Google.Protobuf.WellKnownTypes.Value.ForString(finalPrompt));

    //        // Add image if provided (from sketch)
    //        if (!string.IsNullOrWhiteSpace(imageBase64Data))
    //        {
    //            var imageStruct = new Google.Protobuf.WellKnownTypes.Struct();
    //            imageStruct.Fields.Add("bytesBase64Encoded", Google.Protobuf.WellKnownTypes.Value.ForString(imageBase64Data));
    //            instance.Fields.Add("image", Google.Protobuf.WellKnownTypes.Value.ForStruct(imageStruct));
    //            _logger.LogInformation("Image data added to request for asset {AssetId}", assetId);
    //        }

    //        var instances = new List<Google.Protobuf.WellKnownTypes.Value>
    //    {
    //        Google.Protobuf.WellKnownTypes.Value.ForStruct(instance)
    //    };

    //        // Các thông số phụ (có thể bỏ qua)
    //        var parameters = new Google.Protobuf.WellKnownTypes.Struct();
    //        // parameters.Fields.Add("sampleCount", Google.Protobuf.WellKnownTypes.Value.ForNumber(1)); 
    //        parameters.Fields.Add("sampleCount", Google.Protobuf.WellKnownTypes.Value.ForNumber(1));
    //        // Ép ra khung hình ngang để dàn hàng 4 góc
    //        parameters.Fields.Add("aspectRatio", Google.Protobuf.WellKnownTypes.Value.ForString("16:9"));
    //        // 3. Gọi PredictAsync (dành cho Imagen) thay vì GenerateContentAsync
    //        var response = await _predictionClient.PredictAsync(
    //            endpoint: endpointPath,
    //            instances: instances,
    //            parameters: Google.Protobuf.WellKnownTypes.Value.ForStruct(parameters), // <-- Sửa đúng dòng này
    //            callSettings: null);

    //        // 4. Bóc tách ảnh Base64 từ kết quả trả về của Imagen
    //        string? generatedImageBase64 = null;
    //        if (response.Predictions.Count > 0)
    //        {
    //            var prediction = response.Predictions[0].StructValue;
    //            if (prediction.Fields.TryGetValue("bytesBase64Encoded", out var base64Value))
    //            {
    //                generatedImageBase64 = base64Value.StringValue;
    //            }
    //        }

    //        if (string.IsNullOrWhiteSpace(generatedImageBase64))
    //        {
    //            _logger.LogWarning("No generated image found in response for asset {AssetId}", assetId);
    //            return;
    //        }

    //        _logger.LogInformation("Vertex AI successfully generated image for asset {AssetId}", assetId);

    //        // 5. Giữ nguyên logic Upload S3 cực chuẩn của bạn
    //        var s3Path = await UploadImageToS3Async(
    //            generatedImageBase64,
    //            assetId,
    //            cancellationToken);

    //        if (string.IsNullOrWhiteSpace(s3Path))
    //        {
    //            _logger.LogError("Failed to upload generated image to S3 for asset {AssetId}", assetId);
    //            return;
    //        }

    //        // 6. Cập nhật DB
    //        await UpdateAssetWithGeneratedImageAsync(assetId, s3Path, normalizedPrompt, cancellationToken);
    //        _logger.LogInformation("Asset {AssetId} successfully updated with generated image: {S3Path}", assetId, s3Path);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error calling Vertex AI Imagen for asset {AssetId}", assetId);
    //    }
    //}

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
        string normalizedPrompt,
        CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<CustomDesignDbContext>();
            var assetRepository = scope.ServiceProvider.GetRequiredService<ICustomDesignAssetRepository>();
            var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();

            var asset = await dbContext.CustomDesignAssets
                .FirstOrDefaultAsync(a => a.Id == CustomDesignAssetId.From(assetId), cancellationToken);

            if (asset == null)
            {
                _logger.LogWarning("Asset {AssetId} not found for update", assetId);
                return;
            }

            // Download composite image from S3 and split into 4 views
            var compositeImageUrl = _mediaAssetService.BuildAssetUrl(s3Path);
            var multiviewImagesPaths = await SplitAndUploadCompositeImageAsync(
                compositeImageUrl,
                assetId,
                httpClient,
                cancellationToken);

            if (multiviewImagesPaths == null || multiviewImagesPaths.Count == 0)
            {
                _logger.LogWarning("Failed to split composite image for asset {AssetId}", assetId);
                // Still update with composite image even if split fails
                multiviewImagesPaths = new List<string>();
            }

            // Join paths with comma separator
            var multiviewImagesString = multiviewImagesPaths.Any() ? string.Join(",", multiviewImagesPaths) : null;

            // Update asset with generated image path and normalized prompt
            asset.Update(
                multiviewImages: multiviewImagesString,
                compositeMultiviewImage: s3Path,
                rough3DModel: asset.Rough3DModel,
                rough3DModelTaskId: asset.Rough3DModelTaskId,
                customerPrompt: asset.CustomerPrompt,
                normalizePrompt: normalizedPrompt,
                isNeedSupport: asset.IsNeedSupport,
                isFinalDesign: asset.IsFinalDesign,
                updatedAt: DateTime.UtcNow);

            // Update status to RoughModelGenerating when composite image is generated
            asset.UpdateStatus(CustomDesignAssetStatus.RoughModelGenerating, DateTime.UtcNow);

            assetRepository.Update(asset);
            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Asset {AssetId} updated with composite image: {S3Path}, multiview images: {MultiviewImages}, status: RoughModelGenerating", 
                assetId, s3Path, multiviewImagesString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating asset {AssetId} with generated image", assetId);
        }
    }

    private async Task<List<string>> SplitAndUploadCompositeImageAsync(
        string compositeImageUrl,
        Guid assetId,
        HttpClient httpClient,
        CancellationToken cancellationToken)
    {
        try
        {
            // Download composite image from S3
            var compositeImageBytes = await DownloadImageBytesAsync(compositeImageUrl, httpClient, cancellationToken);
            if (compositeImageBytes == null || compositeImageBytes.Length == 0)
            {
                _logger.LogWarning("Failed to download composite image from {Url}", compositeImageUrl);
                return new List<string>();
            }

            // Split image into 4 parts (2x2 grid)
            var imageParts = SplitImage2x2Grid(compositeImageBytes);
            if (imageParts == null || imageParts.Count != 4)
            {
                _logger.LogWarning("Failed to split composite image into 4 parts for asset {AssetId}", assetId);
                return new List<string>();
            }

            // Upload each part to S3
            var uploadedPaths = new List<string>();
            var viewNames = new[] { "front", "right", "back", "left" };

            for (int i = 0; i < imageParts.Count; i++)
            {
                var fileName = $"custom-design/multiview/{assetId:N}/{viewNames[i]}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.jpg";
                
                var uploadResult = await _mediaService.UploadFileAsync(
                    imageParts[i],
                    fileName,
                    "image/jpeg",
                    cancellationToken);

                if (uploadResult.IsSuccess)
                {
                    uploadedPaths.Add(fileName);
                    _logger.LogInformation("Uploaded {ViewName} image for asset {AssetId}: {Path}", viewNames[i], assetId, fileName);
                }
                else
                {
                    _logger.LogWarning("Failed to upload {ViewName} image for asset {AssetId}: {Error}", viewNames[i], assetId, uploadResult.Error.Message);
                }
            }

            return uploadedPaths;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error splitting and uploading composite image for asset {AssetId}", assetId);
            return new List<string>();
        }
    }

    private async Task<byte[]?> DownloadImageBytesAsync(
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

            return await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading image from {Url}", imageUrl);
            return null;
        }
    }

    private List<byte[]> SplitImage2x2Grid(byte[] compositeImageBytes)
    {
        try
        {
            using var image = Image.Load(compositeImageBytes);

            int width = image.Width;
            int height = image.Height;

            // Each cell is half width and half height
            int cellWidth = width / 2;
            int cellHeight = height / 2;

            var parts = new List<byte[]>();

            // Define rectangles for 2x2 grid: top-left (front), top-right (right), bottom-left (back), bottom-right (left)
            var rectangles = new[]
            {
                new Rectangle(0, 0, cellWidth, cellHeight),                          // top-left: front
                new Rectangle(cellWidth, 0, cellWidth, cellHeight),                  // top-right: right
                new Rectangle(0, cellHeight, cellWidth, cellHeight),                 // bottom-left: back
                new Rectangle(cellWidth, cellHeight, cellWidth, cellHeight)          // bottom-right: left
            };

            foreach (var rect in rectangles)
            {
                using var croppedImage = image.Clone(x => x.Crop(rect));
                
                using var outputStream = new System.IO.MemoryStream();
                croppedImage.SaveAsJpeg(outputStream);
                parts.Add(outputStream.ToArray());
            }

            return parts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error splitting 2x2 grid image");
            return new List<byte[]>();
        }
    }

    private string NormalizePrompt(string originalPrompt)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(originalPrompt))
            {
                return "Generate a high-quality composite product image using 2x2 grid layout";
            }

            // Trim and clean the prompt
            var cleanPrompt = originalPrompt.Trim();

            // Use the standard grid template with the customer prompt
            var normalizedPrompt = $@"A clean 2x2 grid layout showing the same object from four different views. Canvas: - Aspect ratio: 16:9 - Resolution: 1920x1080 Grid: - 2 rows and 2 columns (2x2 grid) - Each cell has equal size (960x540) - No spacing, no padding, no margins between cells - Perfectly aligned grid, sharp boundaries between cells Object placement: - Each cell contains exactly one view of the same object: top-left: front view top-right: right view bottom-left: back view bottom-right: left view Consistency: - The object must have identical scale and size in all four cells - The object must be centered in each cell - Camera distance, angle, and lighting must be consistent across all views - No perspective distortion differences between views Background: - Plain solid background (white or neutral color) - No shadows crossing cell boundaries Constraints: - No overlapping between cells - No extra objects - No text, no watermark - No misalignment, no uneven spacing

The object is: {cleanPrompt}";

            _logger.LogDebug("Normalized prompt for 2x2 grid layout. Original: '{Original}'", originalPrompt);
            return normalizedPrompt;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error normalizing prompt");
            return originalPrompt;
        }
    }
}
