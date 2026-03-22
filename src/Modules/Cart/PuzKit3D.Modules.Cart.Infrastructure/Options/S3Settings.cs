namespace PuzKit3D.Modules.Cart.Infrastructure.Options;

public sealed class S3Settings
{
    public const string ConfigurationSection = nameof(S3Settings);
    public string BucketName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
