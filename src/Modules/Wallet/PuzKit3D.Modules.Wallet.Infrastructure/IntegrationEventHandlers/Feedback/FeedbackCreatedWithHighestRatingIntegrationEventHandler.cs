using PuzKit3D.Contract.Feedback;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.Feedback;

internal sealed class FeedbackCreatedWithHighestRatingIntegrationEventHandler
    : IIntegrationEventHandler<FeedbackCreatedWithHighestRatingIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public FeedbackCreatedWithHighestRatingIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        FeedbackCreatedWithHighestRatingIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        const decimal REWARD_COIN = 5000;

        // Get wallet for this user
        var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == @event.UserId);
        
        if (wallet is null)
        {
            // Wallet doesn't exist, skip (shouldn't happen as it's created when user confirms email)
            return;
        }

        // Add coins to wallet
        var addCoinResult = wallet.AddCoin(REWARD_COIN);
        if (addCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to add coins to wallet: {addCoinResult.Error.Message}");
        }

        // Create wallet transaction record
        var transactionResult = WalletTransaction.Create(
            userId: @event.UserId,
            amount: REWARD_COIN,
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
