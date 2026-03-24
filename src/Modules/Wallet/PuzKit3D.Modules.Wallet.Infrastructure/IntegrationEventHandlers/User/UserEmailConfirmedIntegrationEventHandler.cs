using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using WalletEntity = PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.Wallet;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.User;

internal sealed class UserEmailConfirmedIntegrationEventHandler
    : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public UserEmailConfirmedIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        UserEmailConfirmedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Check if wallet already exists for this user
        var existingWallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == @event.UserId);
        if (existingWallet is not null)
        {
            // Wallet already exists, no need to create
            return;
        }

        // Create a new wallet with initial balance of 0
        var walletResult = WalletEntity.Create(
            userId: @event.UserId,
            initialBalance: 0);

        if (walletResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to create wallet for user {@event.UserId}: {walletResult.Error.Message}");
        }

        var wallet = walletResult.Value;
        await _dbContext.Wallets.AddAsync(wallet, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
