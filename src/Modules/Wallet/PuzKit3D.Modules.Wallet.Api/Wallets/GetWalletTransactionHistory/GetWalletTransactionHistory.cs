using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries;
using PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWalletTransactionHistory;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Wallet.Api.Wallets.GetWalletTransactionHistory;

internal sealed class GetWalletTransactionHistory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapWalletsGroup()
            .MapGet("/wallet/{walletId}/wallet-transactions", async (
                [FromRoute] Guid walletId,
                ISender sender,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetWalletTransactionHistoryQuery(walletId);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetWalletTransactionHistory")
            .WithDescription("Get transaction history for a specific wallet")
            .Produces<List<WalletTransactionDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}
