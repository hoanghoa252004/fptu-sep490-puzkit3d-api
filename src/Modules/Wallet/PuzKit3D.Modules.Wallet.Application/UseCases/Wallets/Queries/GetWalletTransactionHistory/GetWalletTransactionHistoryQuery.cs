using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWalletTransactionHistory;

public sealed record GetWalletTransactionHistoryQuery(Guid WalletId) : IQuery<List<WalletTransactionDto>>;