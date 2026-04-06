using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Queries.GetWalletConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Wallet.Api.WalletConfigs.GetWalletConfig;

internal sealed class GetWalletConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/configs/wallet", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetWalletConfigQuery();
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Wallet Configs")
            .WithName("GetWalletConfig")
            .WithSummary("Get wallet configuration")
            .AllowAnonymous()
            .Produces<GetWalletConfigResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
