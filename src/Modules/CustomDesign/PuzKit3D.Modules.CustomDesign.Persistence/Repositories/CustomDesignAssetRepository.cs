using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class CustomDesignAssetRepository : ICustomDesignAssetRepository
{
    private readonly CustomDesignDbContext _context;

    public CustomDesignAssetRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<CustomDesignAsset?> GetByIdAsync(
        CustomDesignAssetId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<CustomDesignAsset?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .FirstOrDefaultAsync(a => a.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignAsset>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignAsset>> GetByRequestIdAsync(
        CustomDesignRequestId requestId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .Where(a => a.CustomDesignRequestId == requestId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignAsset>> GetFinalDesignsByRequestIdAsync(
        CustomDesignRequestId requestId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .Where(a => a.CustomDesignRequestId == requestId && a.IsFinalDesign)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CustomDesignAsset asset,
        CancellationToken cancellationToken = default)
    {
        await _context.CustomDesignAssets.AddAsync(asset, cancellationToken);
    }

    public void Update(CustomDesignAsset asset)
    {
        _context.CustomDesignAssets.Update(asset);
    }

    public void Delete(CustomDesignAsset asset)
    {
        _context.CustomDesignAssets.Remove(asset);
    }

    public async Task<bool> ExistsByIdAsync(CustomDesignAssetId id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignAssets
            .AnyAsync(a => a.Code == code, cancellationToken);
    }
}
