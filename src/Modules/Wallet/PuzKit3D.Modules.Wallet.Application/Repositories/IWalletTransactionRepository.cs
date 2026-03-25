using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.Repositories;

public interface IWalletTransactionRepository
{
    Task<ResultT<WalletTransaction>> GetByIdAsync(WalletTransactionId id, CancellationToken cancellationToken = default);
    Task<ResultT<IEnumerable<WalletTransaction>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ResultT<IEnumerable<WalletTransaction>>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default);
    Task DeleteAsync(WalletTransactionId id, CancellationToken cancellationToken = default);
}
