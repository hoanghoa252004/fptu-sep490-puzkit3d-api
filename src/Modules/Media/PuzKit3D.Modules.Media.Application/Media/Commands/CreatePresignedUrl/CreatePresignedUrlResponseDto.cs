using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Application.Media.Commands.CreatePresignedUrl;

public sealed record CreatePresignedUrlResponseDto(
    string PresignedUrl,
    string Path
);
