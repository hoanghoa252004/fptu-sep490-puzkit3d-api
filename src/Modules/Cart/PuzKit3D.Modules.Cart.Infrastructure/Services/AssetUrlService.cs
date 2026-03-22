using System.Text.Json;
using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Cart.Application.Services;
using PuzKit3D.Modules.Cart.Infrastructure.Options;

namespace PuzKit3D.Modules.Cart.Infrastructure.Services;

internal sealed class AssetUrlService : IAssetUrlService
{
    private readonly S3Settings _s3Settings;

    public AssetUrlService(IOptions<S3Settings> options)
    {
        _s3Settings = options.Value;
    }

    public string BuildAssetUrl(string assetPath)
    {
        if (string.IsNullOrWhiteSpace(assetPath))
            return string.Empty;

        return $"{_s3Settings.BaseUrl.TrimEnd('/')}/{assetPath.TrimStart('/')}";
    }

    public List<string> BuildAssetUrls(string? assetPaths)
    {
        if (string.IsNullOrWhiteSpace(assetPaths))
            return new List<string>();

        // Try to parse as JSON object first
        try
        {
            using var doc = JsonDocument.Parse(assetPaths);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                // It's a JSON object, extract values only
                var urls = new List<string>();
                foreach (var property in root.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.String)
                    {
                        var value = property.Value.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            urls.Add(BuildAssetUrl(value));
                        }
                    }
                }
                return urls;
            }
        }
        catch
        {
            // Not valid JSON, treat as comma-separated paths
        }

        // Fallback: treat as comma-separated paths
        var paths = assetPaths.Split(',')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        return paths.Select(BuildAssetUrl).ToList();
    }
}
