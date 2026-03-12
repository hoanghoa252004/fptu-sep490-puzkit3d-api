using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public sealed class TransactionId : StronglyTypedId<Guid>
{
    private TransactionId(Guid value) : base(value) { }

    public static TransactionId Create() => new(Guid.NewGuid());

    public static TransactionId From(Guid value) => new(value);
}
