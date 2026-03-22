namespace PuzKit3D.Modules.Cart.Application.Services;

public interface IAssetUrlService
{
    string BuildAssetUrl(string assetPath);
    List<string> BuildAssetUrls(string? assetPaths);
}
