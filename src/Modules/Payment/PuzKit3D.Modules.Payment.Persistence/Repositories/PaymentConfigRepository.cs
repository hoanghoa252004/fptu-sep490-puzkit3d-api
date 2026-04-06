using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.PaymentConfigs;

namespace PuzKit3D.Modules.Payment.Persistence.Repositories;

internal sealed class PaymentConfigRepository : IPaymentConfigRepository
{
    private readonly PaymentDbContext _dbContext;

    public PaymentConfigRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaymentConfig?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentConfigs.FirstOrDefaultAsync(cancellationToken);
    }
}
