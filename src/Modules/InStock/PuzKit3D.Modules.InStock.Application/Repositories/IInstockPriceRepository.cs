using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockPriceRepository : IRepositoryBase<InstockPrice, InstockPriceId>
{
    Task<IEnumerable<InstockPrice>> GetActivePricesAsync(DateTime date, CancellationToken cancellationToken = default);
}
