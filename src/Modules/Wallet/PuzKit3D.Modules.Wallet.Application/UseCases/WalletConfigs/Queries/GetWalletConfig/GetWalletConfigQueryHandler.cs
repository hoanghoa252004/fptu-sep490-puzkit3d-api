using MediatR;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Queries.GetWalletConfig;

internal sealed class GetWalletConfigQueryHandler : IQueryHandler<GetWalletConfigQuery, GetWalletConfigResponse>
{
    private readonly IWalletConfigRepository _walletConfigRepository;

    public GetWalletConfigQueryHandler(IWalletConfigRepository walletConfigRepository)
    {
        _walletConfigRepository = walletConfigRepository;
    }

    public async Task<ResultT<GetWalletConfigResponse>> Handle(
        GetWalletConfigQuery request,
        CancellationToken cancellationToken)
    {
        var walletConfig = await _walletConfigRepository.GetFirstAsync(cancellationToken);

        if (walletConfig is null)
        {
            return Result.Failure<GetWalletConfigResponse>(
                WalletError.WalletConfigNotFound());
        }

        var response = new GetWalletConfigResponse(
            walletConfig.Id,
            walletConfig.OnlineOrderReturnPercentage,
            walletConfig.OnlineOrderCompletedRewardPercentage,
            walletConfig.CODOrderCompletedRewardPercentage,
            walletConfig.UpdatedAt);

        return Result.Success(response);
    }
}
