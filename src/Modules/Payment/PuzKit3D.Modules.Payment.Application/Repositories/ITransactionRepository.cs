using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Application.Repositories;

public interface ITransactionRepository : IRepositoryBase<Transaction, TransactionId>
{
}
