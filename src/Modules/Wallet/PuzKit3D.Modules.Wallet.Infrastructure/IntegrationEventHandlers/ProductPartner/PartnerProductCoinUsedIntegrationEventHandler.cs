using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.ProductPartner;

internal sealed class PartnerProductCoinUsedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductCoinUsedIntegrationEvent>
{
    private readonly WalletDbContext _walletDbContext;

    public PartnerProductCoinUsedIntegrationEventHandler(WalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task HandleAsync(
        PartnerProductCoinUsedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var wallet = await _walletDbContext.Wallets
            .FirstOrDefaultAsync(w => w.UserId == @event.CustomerId, cancellationToken);

        if (wallet == null)
        {
            return;
        }

        if (wallet.Balance < @event.UsedCoinAmount)
        {
            throw new PuzKit3DException("User coin amount exeed balance in wallet");
        }

        //Deduct coins used for this order
        var deductCoinResult = wallet.DeductCoin(@event.UsedCoinAmount);
        if (deductCoinResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to deduct coins from wallet: {deductCoinResult.Error.Message}");
        }

        // Create wallet transaction record for the coin usage
        var transactionResult = WalletTransaction.Create(
            @event.CustomerId,
            @event.UsedCoinAmount,
            WalletTransactionType.Spend,
            @event.OrderId);

        if (transactionResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to create wallet transaction: {transactionResult.Error.Message}");
        }

        var transaction = transactionResult.Value;

        // Update wallet in database
        _walletDbContext.Wallets.Update(wallet);

        // Add transaction record
        await _walletDbContext.WalletTransactions.AddAsync(transaction, cancellationToken);

        // Save changes
        await _walletDbContext.SaveChangesAsync(cancellationToken);
    }
}