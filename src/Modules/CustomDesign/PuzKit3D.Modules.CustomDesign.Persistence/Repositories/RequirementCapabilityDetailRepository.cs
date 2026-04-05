using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class RequirementCapabilityDetailRepository : IRequirementCapabilityDetailRepository
{
    private readonly CustomDesignDbContext _context;

    public RequirementCapabilityDetailRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<RequirementCapabilityDetail?> GetByIdAsync(
        RequirementCapabilityDetailId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.RequirementCapabilityDetails
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<RequirementCapabilityDetail>> GetByRequirementIdAsync(
        CustomDesignRequirementId requirementId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RequirementCapabilityDetails
            .Where(r => r.CustomDesignRequirementId == requirementId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<RequirementCapabilityDetail?> GetByRequirementAndCapabilityAsync(
        CustomDesignRequirementId requirementId,
        Guid capabilityId,
        CancellationToken cancellationToken = default)
    {
        return await _context.RequirementCapabilityDetails
            .FirstOrDefaultAsync(r => r.CustomDesignRequirementId == requirementId && r.CapabilityId == capabilityId, cancellationToken);
    }

    public async Task AddAsync(
        RequirementCapabilityDetail detail,
        CancellationToken cancellationToken = default)
    {
        await _context.RequirementCapabilityDetails.AddAsync(detail, cancellationToken);
    }

    public void Delete(RequirementCapabilityDetail detail)
    {
        _context.RequirementCapabilityDetails.Remove(detail);
    }

    public async Task<bool> ExistsByIdAsync(RequirementCapabilityDetailId id, CancellationToken cancellationToken = default)
    {
        return await _context.RequirementCapabilityDetails
            .AnyAsync(r => r.Id == id, cancellationToken);
    }
}
