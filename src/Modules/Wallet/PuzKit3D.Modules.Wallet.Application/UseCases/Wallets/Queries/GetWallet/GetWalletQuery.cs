using MediatR;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWallet;

public sealed record GetWalletQuery(Guid UserId) : IQuery<WalletResponseDto>;

