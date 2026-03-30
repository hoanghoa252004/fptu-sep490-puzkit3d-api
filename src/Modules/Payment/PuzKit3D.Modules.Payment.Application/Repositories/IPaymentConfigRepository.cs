using PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;

namespace PuzKit3D.Modules.Payment.Application.Repositories;

public interface IPaymentConfigRepository
{
    Task<PaymentConfig?> GetFirstAsync(CancellationToken cancellationToken = default);
}
