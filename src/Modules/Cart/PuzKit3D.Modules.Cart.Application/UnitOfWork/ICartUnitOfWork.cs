namespace PuzKit3D.Modules.Cart.Application.UnitOfWork;

/// <summary>
/// Unit of Work specific for Cart module
/// </summary>
public interface ICartUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
