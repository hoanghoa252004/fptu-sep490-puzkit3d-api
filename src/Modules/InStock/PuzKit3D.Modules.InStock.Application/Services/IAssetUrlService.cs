namespace PuzKit3D.Modules.InStock.Application.Services;

public interface IAssetUrlService
{
    string BuildAssetUrl(string assetPath);
    List<string> BuildAssetUrls(string? assetPaths);
}
