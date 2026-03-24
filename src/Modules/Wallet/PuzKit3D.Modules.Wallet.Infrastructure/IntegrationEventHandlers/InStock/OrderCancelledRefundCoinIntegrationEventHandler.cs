using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.InStock;

internal sealed class OrderCancelledRefundCoinIntegrationEventHandler
    : IIntegrationEventHandler<OrderCancelledRefundCoinIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public OrderCancelledRefundCoinIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        OrderCancelledRefundCoinIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Get wallet for this user
        var wallet = _dbContext.Wallets.FirstOrDefault(w => w.UserId == @event.CustomerId);

        if (wallet is null)
        {
            // Wallet doesn't exist, skip (shouldn't happen)
            return;
        }

        // Refund coins equal to the grand total amount
        var refundCoinResult = wallet.RefundCoin(@event.GrandTotalAmount);
        if (refundCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to refund coins to wallet: {refundCoinResult.Error.Message}");
        }

        // Create wallet transaction record for the refund
        var transactionResult = WalletTransaction.Create(
            userId: @event.CustomerId,
            amount: @event.GrandTotalAmount,
            type: WalletTransactionType.Refund,
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
