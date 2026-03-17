using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockOrderRepository : IRepositoryBase<InstockOrder, InstockOrderId>
{
    Task<InstockOrder?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<InstockOrder?> GetByIdWithDetailsAsync(InstockOrderId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InstockOrder>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InstockOrder>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
}
