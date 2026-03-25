namespace PuzKit3D.SharedKernel.Application.Media;

public interface IMediaAssetService
{
    string BuildAssetUrl(string assetPath);
    List<string> BuildAssetUrls(string? assetPaths);
}
