using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.SharedKernel.Domain.Results;
using WalletEntity = PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.Wallet;

namespace PuzKit3D.Modules.Wallet.Application.Repositories;

public interface IWalletRepository
{
    Task<ResultT<WalletEntity>> GetByIdAsync(WalletId id, CancellationToken cancellationToken = default);
    Task<ResultT<WalletEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(WalletEntity wallet, CancellationToken cancellationToken = default);
    Task UpdateAsync(WalletEntity wallet, CancellationToken cancellationToken = default);
    Task DeleteAsync(WalletId id, CancellationToken cancellationToken = default);
}
