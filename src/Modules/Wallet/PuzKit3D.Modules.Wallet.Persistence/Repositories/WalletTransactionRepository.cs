using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Persistence.Repositories;

internal sealed class WalletTransactionRepository : IWalletTransactionRepository
{
    private readonly WalletDbContext _context;

    public WalletTransactionRepository(WalletDbContext context)
    {
        _context = context;
    }

    public async Task<ResultT<WalletTransaction>> GetByIdAsync(WalletTransactionId id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.WalletTransactions.FirstOrDefaultAsync(wt => wt.Id == id, cancellationToken);

        if (transaction is null)
            return Result.Failure<WalletTransaction>(WalletTransactionError.TransactionNotFound(id.Value));

        return Result.Success(transaction);
    }

    public async Task<ResultT<IEnumerable<WalletTransaction>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var transactions = await _context.WalletTransactions
            .Where(wt => wt.UserId == userId)
            .OrderByDescending(wt => wt.CreatedAt)
            .ToListAsync(cancellationToken);

        return Result.Success(transactions.AsEnumerable());
    }

    public async Task<ResultT<IEnumerable<WalletTransaction>>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var transactions = await _context.WalletTransactions
            .Where(wt => wt.OrderId == orderId)
            .OrderByDescending(wt => wt.CreatedAt)
            .ToListAsync(cancellationToken);

        return Result.Success(transactions.AsEnumerable());
    }

    public async Task AddAsync(WalletTransaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.WalletTransactions.AddAsync(transaction, cancellationToken);
    }

    public async Task DeleteAsync(WalletTransactionId id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.WalletTransactions.FirstOrDefaultAsync(wt => wt.Id == id, cancellationToken);
        if (transaction is not null)
        {
            _context.WalletTransactions.Remove(transaction);
        }
    }
}
