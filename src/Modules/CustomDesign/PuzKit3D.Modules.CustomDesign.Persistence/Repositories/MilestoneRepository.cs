using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class MilestoneRepository : IMilestoneRepository
{
    private readonly CustomDesignDbContext _context;

    public MilestoneRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<Milestone?> GetByIdAsync(MilestoneId id, CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Milestone>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .OrderBy(m => m.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Milestone>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .Where(m => m.IsActive)
            .OrderBy(m => m.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Milestone?> GetBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .FirstOrDefaultAsync(m => m.SequenceOrder == sequenceOrder, cancellationToken);
    }

    public async Task AddAsync(Milestone milestone, CancellationToken cancellationToken = default)
    {
        await _context.Milestones.AddAsync(milestone, cancellationToken);
    }

    public void Update(Milestone milestone)
    {
        _context.Milestones.Update(milestone);
    }

    public void Delete(Milestone milestone)
    {
        _context.Milestones.Remove(milestone);
    }

    public async Task<bool> ExistsByIdAsync(MilestoneId id, CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .AnyAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default)
    {
        return await _context.Milestones
            .AnyAsync(m => m.SequenceOrder == sequenceOrder, cancellationToken);
    }
}
