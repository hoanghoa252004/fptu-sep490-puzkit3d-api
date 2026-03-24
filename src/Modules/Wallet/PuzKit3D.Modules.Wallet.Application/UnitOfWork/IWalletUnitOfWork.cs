namespace PuzKit3D.Modules.Wallet.Application.UnitOfWork;

public interface IWalletUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
