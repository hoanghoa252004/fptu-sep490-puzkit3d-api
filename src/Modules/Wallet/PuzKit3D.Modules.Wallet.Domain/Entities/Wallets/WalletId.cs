using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;

public sealed class WalletId : StronglyTypedId<Guid>
{
    private WalletId(Guid value) : base(value) { }

    public static WalletId Create() => new(Guid.NewGuid());

    public static WalletId From(Guid value) => new(value);
}
