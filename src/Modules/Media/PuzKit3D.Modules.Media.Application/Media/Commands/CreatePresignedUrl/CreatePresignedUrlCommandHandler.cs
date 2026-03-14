using PuzKit3D.Modules.Media.Application.Services;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Application.Media.Commands.CreatePresignedUrl;

public class CreatePresignedUrlCommandHandler : ICommandTHandler<CreatePresignedUrlCommand, CreatePresignedUrlResponseDto>
{
    private readonly IMediaService _mediaService;

    public CreatePresignedUrlCommandHandler(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    public async Task<ResultT<CreatePresignedUrlResponseDto>> Handle(CreatePresignedUrlCommand request, CancellationToken cancellationToken)
    {
        string key = $"{request.Folder}/{request.Path}/{request.FileName}".ToLower();

        var uri = await _mediaService.CreatePresignedUrlAsync(request.ContentType, key);

        return Result.Success(new CreatePresignedUrlResponseDto(uri, key));
    }
}
