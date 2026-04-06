using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletConfigs;

namespace PuzKit3D.Modules.Wallet.Persistence.Repositories;

internal sealed class WalletConfigRepository : IWalletConfigRepository
{
    private readonly WalletDbContext _dbContext;

    public WalletConfigRepository(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletConfig?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.WalletConfigs.FirstOrDefaultAsync(cancellationToken);
    }
}
