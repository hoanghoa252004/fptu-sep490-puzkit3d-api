using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class CustomDesignRequirementRepository : ICustomDesignRequirementRepository
{
    private readonly CustomDesignDbContext _context;

    public CustomDesignRequirementRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<CustomDesignRequirement?> GetByIdAsync(
        CustomDesignRequirementId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<CustomDesignRequirement?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequirement>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CustomDesignRequirement>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CustomDesignRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        await _context.CustomDesignRequirements.AddAsync(requirement, cancellationToken);
    }

    public void Update(CustomDesignRequirement requirement)
    {
        _context.CustomDesignRequirements.Update(requirement);
    }

    public void Delete(CustomDesignRequirement requirement)
    {
        _context.CustomDesignRequirements.Remove(requirement);
    }

    public async Task<bool> ExistsByIdAsync(CustomDesignRequirementId id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.CustomDesignRequirements
            .AnyAsync(r => r.Code == code, cancellationToken);
    }
}
