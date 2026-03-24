using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.SharedKernel.Domain.Results;
using WalletEntity = PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.Wallet;

namespace PuzKit3D.Modules.Wallet.Persistence.Repositories;

internal sealed class WalletRepository : IWalletRepository
{
    private readonly WalletDbContext _context;

    public WalletRepository(WalletDbContext context)
    {
        _context = context;
    }

    public async Task<ResultT<WalletEntity>> GetByIdAsync(WalletId id, CancellationToken cancellationToken = default)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if (wallet is null)
            return Result.Failure<WalletEntity>(WalletError.WalletNotFound(id.Value));

        return Result.Success(wallet);
    }

    public async Task<ResultT<WalletEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);

        if (wallet is null)
            return Result.Failure<WalletEntity>(WalletError.WalletNotFoundForUser(userId));

        return Result.Success(wallet);
    }

    public async Task AddAsync(WalletEntity wallet, CancellationToken cancellationToken = default)
    {
        await _context.Wallets.AddAsync(wallet, cancellationToken);
    }

    public async Task UpdateAsync(WalletEntity wallet, CancellationToken cancellationToken = default)
    {
        _context.Wallets.Update(wallet);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(WalletId id, CancellationToken cancellationToken = default)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        if (wallet is not null)
        {
            _context.Wallets.Remove(wallet);
        }
    }
}
