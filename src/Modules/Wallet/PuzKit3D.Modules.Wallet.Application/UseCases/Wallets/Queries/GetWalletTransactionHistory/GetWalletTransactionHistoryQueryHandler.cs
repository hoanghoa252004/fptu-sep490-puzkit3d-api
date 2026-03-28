using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWalletTransactionHistory;

internal sealed class GetWalletTransactionHistoryQueryHandler 
    : IQueryHandler<GetWalletTransactionHistoryQuery, List<WalletTransactionDto>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletTransactionRepository _walletTransactionRepository;

    public GetWalletTransactionHistoryQueryHandler(
        IWalletRepository walletRepository,
        IWalletTransactionRepository walletTransactionRepository)
    {
        _walletRepository = walletRepository;
        _walletTransactionRepository = walletTransactionRepository;
    }

    public async Task<ResultT<List<WalletTransactionDto>>> Handle(
        GetWalletTransactionHistoryQuery request,
        CancellationToken cancellationToken)
    {
        // Get wallet to verify it exists
        var walletResult = await _walletRepository.GetByIdAsync(
            PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.WalletId.From(request.WalletId),
            cancellationToken);

        if (walletResult.IsFailure)
            return Result.Failure<List<WalletTransactionDto>>(walletResult.Error);

        var wallet = walletResult.Value;

        // Get transactions for this wallet (using wallet's userId)
        var transactionsResult = await _walletTransactionRepository.GetByUserIdAsync(
            wallet.UserId,
            cancellationToken);

        if (transactionsResult.IsFailure)
            return Result.Failure<List<WalletTransactionDto>>(transactionsResult.Error);

        var transactionDtos = transactionsResult.Value
            .Select(t => new WalletTransactionDto(
                t.Id.Value,
                t.UserId,
                t.Amount,
                t.Type.ToString(),
                t.OrderId,
                t.CreatedAt))
            .ToList();

        return Result.Success(transactionDtos);
    }
}
