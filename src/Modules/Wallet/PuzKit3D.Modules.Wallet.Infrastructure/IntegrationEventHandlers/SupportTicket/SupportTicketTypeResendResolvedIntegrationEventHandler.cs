using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Application.UnitOfWork;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.SupportTicket;

internal sealed class SupportTicketTypeResendResolvedIntegrationEventHandler : IIntegrationEventHandler<SupportTicketTypeResendResolvedIntegrationEvent>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletConfigRepository _walletConfigRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository;
    private readonly IWalletUnitOfWork _unitOfWork;

    public SupportTicketTypeResendResolvedIntegrationEventHandler(
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

    public async Task HandleAsync(SupportTicketTypeResendResolvedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var wallet = (await _walletRepository.GetByUserIdAsync(@event.CustomerId)).Value;

        if (wallet is null)
        {
            // Wallet doesn't exist, skip (shouldn't happen)
            return;
        }

        // Get wallet config to determine reward percentage
        var walletConfig = await _walletConfigRepository.GetFirstAsync(cancellationToken);
        var orderReturnPercentage = walletConfig?.OnlineOrderReturnPercentage ?? 80m;

        decimal refundAmount = (@event.GrandTotalAmount) * (orderReturnPercentage / 100m);
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
