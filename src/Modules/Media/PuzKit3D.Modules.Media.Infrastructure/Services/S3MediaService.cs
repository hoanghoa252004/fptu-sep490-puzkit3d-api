using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Media.Application.Services;
using PuzKit3D.Modules.Media.Infrastructure.DependencyInjection.Options;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Infrastructure.Services;

public sealed class S3MediaService : IMediaService
{
    private readonly IAmazonS3 _s3;
    private readonly S3Settings _s3Settings;

    public S3MediaService(IAmazonS3 s3, IOptions<S3Settings> options)
    {
        _s3 = s3;
        _s3Settings = options.Value;
    }

    public async Task<string> CreatePresignedUrlAsync(string contentType, string key)
    {
        var request = new GetPreSignedUrlRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(5),
                ContentType = contentType
            };

        return await _s3.GetPreSignedURLAsync(request);
    }
}
