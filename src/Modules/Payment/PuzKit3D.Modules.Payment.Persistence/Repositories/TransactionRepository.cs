using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.Repositories;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Payment.Persistence.Repositories;

internal sealed class TransactionRepository : ITransactionRepository
{
    private readonly PaymentDbContext _context;

    public TransactionRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(
        TransactionId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> FindAsync(
        Expression<Func<Transaction, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> FindAsyncTracking(
        Expression<Func<Transaction, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(predicate)
            .AsTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(Transaction entity)
    {
        _context.Transactions.Add(entity);
    }

    public void Update(Transaction entity)
    {
        _context.Transactions.Update(entity);
    }

    public void Delete(Transaction entity)
    {
        _context.Transactions.Remove(entity);
    }

    public void DeleteMultiple(List<Transaction> entities)
    {
        _context.Transactions.RemoveRange(entities);
    }
}
