using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Queries.GetInstockOrderConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockOrderConfigs.GetInstockOrderConfig;

internal sealed class GetInstockOrderConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapGet("/config", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockOrderConfigQuery();
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Instock Order Configs")
            .WithName("GetInstockOrderConfig")
            .WithSummary("Get instock order configuration")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator, Roles.BusinessManager))
            .Produces<GetInstockOrderConfigResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
