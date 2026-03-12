namespace PuzKit3D.Modules.Payment.Application.UnitOfWork;

/// <summary>
/// Unit of Work specific for Payment module
/// </summary>
public interface IPaymentUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
