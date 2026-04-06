using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Application.Services;

public interface IMediaService
{
    Task<string> CreatePresignedUrlAsync(string contentType, string key);
    Task<ResultT<string>> UploadFileAsync(byte[] fileData, string key, string contentType, CancellationToken cancellationToken = default);
}
