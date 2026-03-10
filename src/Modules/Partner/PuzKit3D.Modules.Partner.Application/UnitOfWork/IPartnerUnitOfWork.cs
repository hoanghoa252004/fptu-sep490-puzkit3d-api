namespace PuzKit3D.Modules.Partner.Application.UnitOfWork;

public interface IPartnerUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
