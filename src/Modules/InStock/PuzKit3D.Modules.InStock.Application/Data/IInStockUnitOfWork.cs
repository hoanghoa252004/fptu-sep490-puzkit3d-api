namespace PuzKit3D.Modules.InStock.Application.Data;

/// <summary>
/// Unit of Work specific for InStock module
/// </summary>
public interface IInStockUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
