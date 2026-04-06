using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetsByRequestId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignAssets.GetAssetsByRequestId;

internal sealed class GetCustomDesignAssetsByRequestId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup($"/api/custom-design-requests")
            .MapGet("/{requestId}/custom-design-assets", async (
                Guid requestId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetCustomDesignAssetsByRequestIdQuery(requestId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Custom Design Assets")
            .WithName("GetCustomDesignAssetsByRequestId")
            .WithSummary("Get all assets for a request")
            .WithDescription("Gets all custom design assets for a specific request, ordered by version")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff, Roles.BusinessManager))
            .Produces<List<GetCustomDesignAssetsByRequestIdResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
