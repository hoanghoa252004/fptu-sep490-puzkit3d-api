using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Media.Application.Media.Commands.CreatePresignedUrl;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Api.Media.CreatePresignedUrl;

public class CreatePresignedUrl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/uploads/presigned-url", async ([FromBody] CreatePresignedUrlRequestDto request, ISender _sender, CancellationToken cancellationToken) =>
            {
                var command = new CreatePresignedUrlCommand(request.ContentType, request.Folder, request.Path, request.FileName);

                var result = await _sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithTags("Media")
            .WithName("CreatePresignedUrl")
            //.RequireAuthorization()
            .AllowAnonymous()
            .Produces<CreatePresignedUrlResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record CreatePresignedUrlRequestDto(
    string ContentType,
    string Folder,
    string Path,
    string FileName
);