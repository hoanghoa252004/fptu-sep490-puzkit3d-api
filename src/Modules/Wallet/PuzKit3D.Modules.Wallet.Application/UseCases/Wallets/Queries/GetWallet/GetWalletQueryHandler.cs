using MediatR;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries.GetWallet;

internal sealed class GetWalletQueryHandler : IQueryHandler<GetWalletQuery, WalletResponseDto>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<ResultT<WalletResponseDto>> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {

        var walletResult = await _walletRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (walletResult.IsFailure)
            return Result.Failure<WalletResponseDto>(walletResult.Error);

        var wallet = walletResult.Value;
        var walletDto = new WalletResponseDto(
            wallet.Id.Value,
            wallet.UserId,
            wallet.Balance,
            wallet.CreatedAt,
            wallet.UpdatedAt);

        return Result.Success(walletDto);
    }
}
