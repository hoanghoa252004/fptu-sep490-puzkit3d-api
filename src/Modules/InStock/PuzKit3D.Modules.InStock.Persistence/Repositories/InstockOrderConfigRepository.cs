using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderConfigs;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockOrderConfigRepository : IInstockOrderConfigRepository
{
    private readonly InStockDbContext _dbContext;

    public InstockOrderConfigRepository(InStockDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InstockOrderConfig?> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.InstockOrderConfigs.FirstOrDefaultAsync(cancellationToken);
    }
}
