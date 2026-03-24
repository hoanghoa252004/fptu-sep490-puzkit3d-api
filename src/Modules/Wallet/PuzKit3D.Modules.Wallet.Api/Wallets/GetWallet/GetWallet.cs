using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries;
using PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWallet;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.Wallet.Api.Wallets.GetWallet;

internal sealed class GetWallet : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapWalletsGroup()
            .MapGet("/wallet", async (
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken = default) =>
            {
                var userId = Guid.Parse(currentUser.UserId!);
                var query = new GetWalletQuery(userId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetWallet")
            .WithDescription("Get current user's wallet information")
            .Produces<WalletResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer));
    }
}
