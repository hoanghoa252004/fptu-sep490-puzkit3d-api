using PuzKit3D.Modules.InStock.Domain.Entities.Orders;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IOrderRepository : IRepositoryBase<Order, OrderId>
{
    Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default);
}
