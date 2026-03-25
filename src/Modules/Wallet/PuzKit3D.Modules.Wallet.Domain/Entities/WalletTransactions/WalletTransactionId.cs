using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;

public sealed class WalletTransactionId : StronglyTypedId<Guid>
{
    private WalletTransactionId(Guid value) : base(value) { }

    public static WalletTransactionId Create() => new(Guid.NewGuid());

    public static WalletTransactionId From(Guid value) => new(value);
}
