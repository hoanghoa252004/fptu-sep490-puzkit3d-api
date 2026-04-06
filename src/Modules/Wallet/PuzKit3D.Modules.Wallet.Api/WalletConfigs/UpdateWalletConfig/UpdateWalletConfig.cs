using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Commands.UpdateWalletConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Wallet.Api.WalletConfigs.UpdateWalletConfig;

internal sealed class UpdateWalletConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/configs/wallet", async (
                UpdateWalletConfigRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateWalletConfigCommand(
                    request.OnlineOrderReturnPercentage,
                    request.OnlineOrderCompletedRewardPercentage,
                    request.CODOrderCompletedRewardPercentage);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Wallet Configs")
            .WithName("UpdateWalletConfig")
            .WithSummary("Update wallet configuration")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record UpdateWalletConfigRequest(
    decimal? OnlineOrderReturnPercentage,
    decimal? OnlineOrderCompletedRewardPercentage,
    decimal? CODOrderCompletedRewardPercentage);
