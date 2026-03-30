using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Commands.UpdateWalletConfig;

public record UpdateWalletConfigCommand(
    decimal? OnlineOrderReturnPercentage,
    decimal? OnlineOrderCompletedRewardPercentage,
    decimal? CODOrderCompletedRewardPercentage) : ICommand;
