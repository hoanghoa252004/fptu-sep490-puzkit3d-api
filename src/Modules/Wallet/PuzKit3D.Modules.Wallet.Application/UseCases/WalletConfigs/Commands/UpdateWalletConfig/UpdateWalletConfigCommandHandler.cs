using MediatR;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Application.UnitOfWork;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Commands.UpdateWalletConfig;

internal sealed class UpdateWalletConfigCommandHandler : ICommandHandler<UpdateWalletConfigCommand>
{
    private readonly IWalletConfigRepository _walletConfigRepository;
    private readonly IWalletUnitOfWork _unitOfWork;

    public UpdateWalletConfigCommandHandler(
        IWalletConfigRepository walletConfigRepository,
        IWalletUnitOfWork unitOfWork)
    {
        _walletConfigRepository = walletConfigRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateWalletConfigCommand request,
        CancellationToken cancellationToken)
    {
        var walletConfig = await _walletConfigRepository.GetFirstAsync(cancellationToken);

        if (walletConfig is null)
        {
            return Result.Failure(
                WalletError.WalletConfigNotFound());
        }

        // Update only the fields that are provided
        var updatedOnlineReturn = request.OnlineOrderReturnPercentage ?? walletConfig.OnlineOrderReturnPercentage;
        var updatedOnlineReward = request.OnlineOrderCompletedRewardPercentage ?? walletConfig.OnlineOrderCompletedRewardPercentage;
        var updatedCODReward = request.CODOrderCompletedRewardPercentage ?? walletConfig.CODOrderCompletedRewardPercentage;

        // Validate all percentages must be between 0 and 100
        if (updatedOnlineReturn < 0 || updatedOnlineReturn > 100)
        {
            return Result.Failure(
                WalletError.InvalidOnlineOrderReturnPercentage());
        }

        if (updatedOnlineReward < 0 || updatedOnlineReward > 100)
        {
            return Result.Failure(
                WalletError.InvalidOnlineOrderCompletedRewardPercentage());
        }

        if (updatedCODReward < 0 || updatedCODReward > 100)
        {
            return Result.Failure(
                WalletError.InvalidCODOrderCompletedRewardPercentage());
        }

        walletConfig.Update(updatedOnlineReturn, updatedOnlineReward, updatedCODReward);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
