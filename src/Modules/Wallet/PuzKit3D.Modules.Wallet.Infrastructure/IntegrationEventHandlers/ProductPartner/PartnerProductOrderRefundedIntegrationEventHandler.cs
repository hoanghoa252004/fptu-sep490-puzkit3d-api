using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.ProductPartner;

internal sealed class PartnerProductOrderRefundedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderRefundedIntegrationEvent>
{
    private readonly WalletDbContext _dbContext;

    public PartnerProductOrderRefundedIntegrationEventHandler(WalletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(PartnerProductOrderRefundedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == @event.CustomerId);

        if (wallet == null)
        {
            return;
        }

        decimal refundAmount = (@event.GrandTotalAmount + @event.UserCoinAmount) * @event.PercentRefund;

        if(refundAmount <= 0)
        {
            return;
        }

        // Refund coins equal to the grand total amount
        var refundCoinResult = wallet.RefundCoin(refundAmount);
        if (refundCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to refund coins to wallet: {refundCoinResult.Error.Message}");
        }

        // Create wallet transaction record for the refund
        var transactionResult = WalletTransaction.Create(
            userId: @event.CustomerId,
            amount: refundAmount,
            type: WalletTransactionType.Refund,
            orderId: @event.OrderId
        );

        if(transactionResult.IsFailure)
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
