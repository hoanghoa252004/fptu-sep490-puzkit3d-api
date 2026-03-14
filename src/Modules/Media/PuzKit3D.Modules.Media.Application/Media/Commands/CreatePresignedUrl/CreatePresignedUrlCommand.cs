using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Application.Media.Commands.CreatePresignedUrl;

public sealed record CreatePresignedUrlCommand (
    string ContentType,
    string Folder,
    string Path,
    string FileName
    ) : ICommandT<CreatePresignedUrlResponseDto>;
