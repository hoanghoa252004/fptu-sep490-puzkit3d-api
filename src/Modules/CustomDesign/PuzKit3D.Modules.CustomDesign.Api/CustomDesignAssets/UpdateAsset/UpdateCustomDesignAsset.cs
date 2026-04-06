using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.UpdateAsset;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignAssets.UpdateAsset;

internal sealed class UpdateCustomDesignAsset : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignAssetGroup()
            .MapPut("/{id}", async (
                Guid id,
                UpdateCustomDesignAssetRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCustomDesignAssetCommand(
                    id,
                    request.MultiviewImages,
                    request.IsNeedSupport,
                    request.IsFinalDesign);

                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithName("UpdateCustomDesignAsset")
            .WithSummary("Update custom design asset")
            .WithDescription("Updates custom design asset. Can update MultiviewImages (as array of paths), IsNeedSupport, and IsFinalDesign. If trying to set boolean fields to the same value, returns 400.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public sealed record UpdateCustomDesignAssetRequestDto(
    List<string>? MultiviewImages,
    bool? IsNeedSupport,
    bool? IsFinalDesign);
