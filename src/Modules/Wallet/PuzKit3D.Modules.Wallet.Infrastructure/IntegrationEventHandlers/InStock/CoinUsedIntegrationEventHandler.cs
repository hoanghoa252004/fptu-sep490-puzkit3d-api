using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class CoinUsedIntegrationEventHandler
    : IIntegrationEventHandler<CoinUsedIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public CoinUsedIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        CoinUsedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Get wallet for this user
        var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == @event.CustomerId);

        if (wallet is null)
        {
            // Wallet doesn't exist, skip (shouldn't happen)
            return;
        }

        if (wallet.Balance < @event.UsedCoinAmount)
            throw new PuzKit3DException("User coin amount exeed balance in wallet");

        // Deduct coins used for this order
        var deductCoinResult = wallet.DeductCoin(@event.UsedCoinAmount);
        if (deductCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to deduct coins from wallet: {deductCoinResult.Error.Message}");
        }

        // Create wallet transaction record for the coin usage
        var transactionResult = WalletTransaction.Create(
            userId: @event.CustomerId,
            amount: @event.UsedCoinAmount,
            type: WalletTransactionType.Spend,
            orderId: @event.OrderId);

        if (transactionResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to create wallet transaction: {transactionResult.Error.Message}");
        }

        var transaction = transactionResult.Value;

        // Update wallet in database
        _dbContext.Wallets.Update(wallet);

        // Add transaction record
        await _dbContext.WalletTransactions.AddAsync(transaction, cancellationToken);

        // Save changes
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
