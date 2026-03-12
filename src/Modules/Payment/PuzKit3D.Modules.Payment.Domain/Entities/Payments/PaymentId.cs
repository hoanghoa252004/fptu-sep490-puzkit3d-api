using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Payments;

public sealed class PaymentId : StronglyTypedId<Guid>
{
    private PaymentId(Guid value) : base(value) { }

    public static PaymentId Create() => new(Guid.NewGuid());

    public static PaymentId From(Guid value) => new(value);
}
