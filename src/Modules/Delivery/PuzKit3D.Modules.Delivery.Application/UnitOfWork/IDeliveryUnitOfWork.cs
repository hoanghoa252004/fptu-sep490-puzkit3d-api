using PuzKit3D.Modules.Delivery.Application.Repositories;

namespace PuzKit3D.Modules.Delivery.Application.UnitOfWork;

public interface IDeliveryUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
