using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderConfigs;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockOrderConfigRepository
{
    Task<InstockOrderConfig?> GetFirstAsync(CancellationToken cancellationToken = default);
}
