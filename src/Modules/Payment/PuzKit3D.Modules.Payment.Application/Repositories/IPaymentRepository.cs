using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Application.Repositories;

public interface IPaymentRepository : IRepositoryBase<Domain.Entities.Payments.Payment, PaymentId>
{
    Task<Domain.Entities.Payments.Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}
