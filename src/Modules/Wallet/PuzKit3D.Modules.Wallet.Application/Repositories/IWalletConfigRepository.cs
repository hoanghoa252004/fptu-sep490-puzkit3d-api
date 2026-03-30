using PuzKit3D.Modules.Wallet.Domain.Entities.WalletConfigs;

namespace PuzKit3D.Modules.Wallet.Application.Repositories;

public interface IWalletConfigRepository
{
    Task<WalletConfig?> GetFirstAsync(CancellationToken cancellationToken = default);
}
