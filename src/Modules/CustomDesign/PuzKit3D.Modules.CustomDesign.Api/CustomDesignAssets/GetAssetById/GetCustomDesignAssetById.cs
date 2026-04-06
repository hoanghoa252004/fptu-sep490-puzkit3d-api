using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignAssets.GetAssetById;

internal sealed class GetCustomDesignAssetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignAssetGroup()
            .MapGet("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCustomDesignAssetByIdQuery(id);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetCustomDesignAssetById")
            .WithSummary("Get custom design asset by ID")
            .WithDescription("Gets a specific custom design asset by ID with all its details")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<GetCustomDesignAssetByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
