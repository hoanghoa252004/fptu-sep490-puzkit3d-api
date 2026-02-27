namespace PuzKit3D.Modules.Catalog.Application.Data;

/// <summary>
/// Unit of Work specific for Catalog module
/// </summary>
public interface ICatalogUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
