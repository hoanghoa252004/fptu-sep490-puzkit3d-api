using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Payment.Persistence.Repositories;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Payments.Payment?> GetByIdAsync(
        PaymentId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Domain.Entities.Payments.Payment?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.ReferenceOrderId == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Payments.Payment>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Payments.Payment>> FindAsync(
        Expression<Func<Domain.Entities.Payments.Payment, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(Domain.Entities.Payments.Payment entity)
    {
        _context.Payments.Add(entity);
    }

    public void Update(Domain.Entities.Payments.Payment entity)
    {
        _context.Payments.Update(entity);
    }

    public void Delete(Domain.Entities.Payments.Payment entity)
    {
        _context.Payments.Remove(entity);
    }

    public void DeleteMultiple(List<Domain.Entities.Payments.Payment> entities)
    {
        _context.Payments.RemoveRange(entities);
    }
}
