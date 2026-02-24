using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Repositories;

public interface IOrderRepository : IRepositoryBase<Entities.Orders.Order, Entities.Orders.OrderId>
{
    Task<Entities.Orders.Order?> GetByIdAsync(Entities.Orders.OrderId id, CancellationToken cancellationToken = default);
}
