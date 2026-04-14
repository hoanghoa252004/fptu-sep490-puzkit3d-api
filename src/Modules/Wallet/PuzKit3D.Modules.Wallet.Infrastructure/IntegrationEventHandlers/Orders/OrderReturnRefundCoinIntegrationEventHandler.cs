using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Wallet;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Application.UnitOfWork;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.Orders;

public sealed class OrderReturnRefundCoinIntegrationEventHandler : IIntegrationEventHandler<OrderReturnRefundCoinIntegrationEvent>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletConfigRepository _walletConfigRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository;
    private readonly IWalletUnitOfWork _unitOfWork;

    public OrderReturnRefundCoinIntegrationEventHandler(
        IWalletRepository walletRepository,
        IWalletConfigRepository walletConfigRepository,
        IWalletTransactionRepository walletTransactionRepository,
        IWalletUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletConfigRepository = walletConfigRepository;
        _walletTransactionRepository = walletTransactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        OrderReturnRefundCoinIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Get WalletConfig from repository, use default 80% if not found
        var walletConfig = await _walletConfigRepository.GetFirstAsync(cancellationToken);
        var onlineOrderReturnPercentage = walletConfig?.OnlineOrderReturnPercentage ?? 80m;

        // Find user's wallet by userId
        //var walletResult = await _walletRepository.GetByUserIdAsync(@event.UserId, cancellationToken);

        //Domain.Entities.Wallets.Wallet walletEntity;
        //if (walletResult.IsFailure || walletResult.Value == null)
        //{
        //    // Create wallet if it doesn't exist
        //    var createWalletResult = Domain.Entities.Wallets.Wallet.Create(@event.OrderId);
        //    if (createWalletResult.IsFailure)
        //        return;

        //    walletEntity = createWalletResult.Value;
        //    await _walletRepository.AddAsync(walletEntity, cancellationToken);
        //}
        //else
        //{
        //    walletEntity = walletResult.Value;
        //}
        // Get wallet for this user
        var wallet = (await _walletRepository.GetByUserIdAsync(@event.UserId)).Value;

        if (wallet is null)
        {
            // Wallet doesn't exist, skip (shouldn't happen)
            return;
        }

        // Refund based on payment method and config
        decimal refundAmount = 0;

        if (@event.PaymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase) || @event.PaymentMethod.Equals("COIN", StringComparison.OrdinalIgnoreCase))
        {
            // Refund with percentage from config
            refundAmount = (@event.GrandTotalAmount + @event.UsedCoinAmount) * (onlineOrderReturnPercentage / 100m);
        }
        else if (@event.PaymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase))
        {
            refundAmount = @event.UsedCoinAmount - @event.ShippingFee;
        }

        if (refundAmount <= 0)
            return;

        // Add coins to wallet (refund)
        var refundResult = wallet.RefundCoin(refundAmount);
        if (refundResult.IsFailure)
            return;

        // Create transaction record
        var transactionResult = WalletTransaction.Create(
            wallet.UserId,
            refundAmount,
            WalletTransactionType.Refund,
            @event.OrderId);

        if (transactionResult.IsFailure)
            return;

        await _walletTransactionRepository.AddAsync(transactionResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
