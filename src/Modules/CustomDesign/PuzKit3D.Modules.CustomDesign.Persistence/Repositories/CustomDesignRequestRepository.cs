using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class CustomDesignRequestRepository : ICustomDesignRequestRepository
{
    private readonly CustomDesignDbContext _context;

    public CustomDesignRequestRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<CustomDesignRequest?> GetByIdAsync(
        CustomDesignRequestId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<CustomDesignRequest?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequest>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequest>> GetByRequirementIdAsync(
        CustomDesignRequirementId requirementId,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .Where(r => r.CustomDesignRequirementId == requirementId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequest>> GetByStatusAsync(
        CustomDesignRequestStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .Where(r => r.Status == status)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CustomDesignRequest request,
        CancellationToken cancellationToken = default)
    {
        await _context.CustomDesignRequests.AddAsync(request, cancellationToken);
    }

    public void Update(CustomDesignRequest request)
    {
        _context.CustomDesignRequests.Update(request);
    }

    public void Delete(CustomDesignRequest request)
    {
        _context.CustomDesignRequests.Remove(request);
    }

    public async Task<bool> ExistsByIdAsync(CustomDesignRequestId id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequests
            .AnyAsync(r => r.Code == code, cancellationToken);
    }
}
