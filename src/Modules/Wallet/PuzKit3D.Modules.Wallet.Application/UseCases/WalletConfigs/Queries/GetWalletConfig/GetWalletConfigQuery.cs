using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.WalletConfigs.Queries.GetWalletConfig;

public record GetWalletConfigQuery : IQuery<GetWalletConfigResponse>;

public record GetWalletConfigResponse(
    Guid Id,
    decimal OnlineOrderReturnPercentage,
    decimal OnlineOrderCompletedRewardPercentage,
    decimal CODOrderCompletedRewardPercentage,
    DateTime UpdatedAt);
