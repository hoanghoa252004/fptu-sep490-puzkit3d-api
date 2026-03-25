using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Media;

public sealed class S3Settings
{
    public const string ConfigurationSection = nameof(S3Settings);
    public string BucketName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
}
