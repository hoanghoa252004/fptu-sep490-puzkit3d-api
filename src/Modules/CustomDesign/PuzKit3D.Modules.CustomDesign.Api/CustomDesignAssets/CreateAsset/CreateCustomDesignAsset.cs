using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.CreateAsset;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignAssets.CreateAsset;

internal sealed class CreateCustomDesignAsset : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignAssetGroup()
            .MapPost("", async (
                CreateCustomDesignAssetRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCustomDesignAssetCommand(
                    request.RequestId,
                    request.CustomerPrompt);

                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk(id => Results.Created($"/api/custom-design-assets/{id}", id));

            })
            .WithName("CreateCustomDesignAsset")
            .WithSummary("Create custom design asset")
            .WithDescription("Creates a new custom design asset for a request. Maximum 3 free assets per request. Client sends RequestId and CustomerPrompt. Asset code is auto-generated, and version is calculated based on existing assets for the request.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public sealed record CreateCustomDesignAssetRequestDto(
    Guid RequestId,
    string CustomerPrompt);

