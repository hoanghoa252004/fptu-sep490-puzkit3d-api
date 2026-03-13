using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Domain;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Payment.Application.Repositories;

public interface ITransactionRepository : IRepositoryBase<Transaction, TransactionId>
{
    Task<IEnumerable<Transaction>> FindAsyncTracking(
        Expression<Func<Transaction, bool>> predicate,
        CancellationToken cancellationToken = default);
}
