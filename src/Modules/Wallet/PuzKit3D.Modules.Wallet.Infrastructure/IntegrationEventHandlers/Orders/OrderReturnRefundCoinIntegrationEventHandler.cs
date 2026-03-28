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
    private readonly IWalletTransactionRepository _walletTransactionRepository;
    private readonly IWalletUnitOfWork _unitOfWork;

    public OrderReturnRefundCoinIntegrationEventHandler(
        IWalletRepository walletRepository,
        IWalletTransactionRepository walletTransactionRepository,
        IWalletUnitOfWork unitOfWork)
    {
        _walletRepository = walletRepository;
        _walletTransactionRepository = walletTransactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        OrderReturnRefundCoinIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Find user's wallet by userId
        var walletResult = await _walletRepository.GetByUserIdAsync(@event.OrderId, cancellationToken);

        Domain.Entities.Wallets.Wallet walletEntity;
        if (walletResult.IsFailure || walletResult.Value == null)
        {
            // Create wallet if it doesn't exist
            var createWalletResult = Domain.Entities.Wallets.Wallet.Create(@event.OrderId);
            if (createWalletResult.IsFailure)
                return;

            walletEntity = createWalletResult.Value;
            await _walletRepository.AddAsync(walletEntity, cancellationToken);
        }
        else
        {
            walletEntity = walletResult.Value;
        }

        // Refund 80% of the order amount as coins
        var refundAmount = @event.GrandTotalAmount * 0.8m;

        // Add coins to wallet (refund)
        var refundResult = walletEntity.RefundCoin(refundAmount);
        if (refundResult.IsFailure)
            return;

        // Create transaction record
        var transactionResult = WalletTransaction.Create(
            walletEntity.UserId,
            refundAmount,
            WalletTransactionType.Refund,
            @event.OrderId);

        if (transactionResult.IsFailure)
            return;

        await _walletTransactionRepository.AddAsync(transactionResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
