using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class InstockOrderCompletedCoinRewardIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCompletedCoinRewardIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public InstockOrderCompletedCoinRewardIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        InstockOrderCompletedCoinRewardIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Get wallet for this user
        var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == @event.CustomerId);

        if (wallet is null)
        {
            // Wallet doesn't exist, skip
            return;
        }

        // Get wallet config to determine reward percentage
        var walletConfig = _dbContext.WalletConfigs.FirstOrDefault();

        if (walletConfig is null)
        {
            // Wallet config doesn't exist, skip
            return;
        }

        decimal rewardCoin = 0;

        // Determine reward based on payment method
        if (@event.PaymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase))
        {
            // Calculate reward as percentage of grand total
            rewardCoin = @event.GrandTotalAmount * (walletConfig.OnlineOrderCompletedRewardPercentage / 100);
        }
        else if (@event.PaymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase))
        {
            // Calculate reward as percentage of grand total
            rewardCoin = @event.GrandTotalAmount * (walletConfig.CODOrderCompletedRewardPercentage / 100);
        }
        else if (@event.PaymentMethod.Equals("COIN", StringComparison.OrdinalIgnoreCase))
        {
            // No reward for COIN payment method
            return;
        }

        if (rewardCoin <= 0)
        {
            // No reward to add, skip
            return;
        }

        // Add coins reward to wallet
        var addCoinResult = wallet.AddCoin(rewardCoin);
        if (addCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to add reward coins to wallet: {addCoinResult.Error.Message}");
        }

        // Create wallet transaction record for the reward
        var transactionResult = WalletTransaction.Create(
            userId: @event.CustomerId,
            amount: rewardCoin,
            type: WalletTransactionType.Reward,
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
