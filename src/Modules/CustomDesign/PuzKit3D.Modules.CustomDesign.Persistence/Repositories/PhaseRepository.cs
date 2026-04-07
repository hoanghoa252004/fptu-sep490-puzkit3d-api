using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class PhaseRepository : IPhaseRepository
{
    private readonly CustomDesignDbContext _context;

    public PhaseRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<Phase?> GetByIdAsync(PhaseId id, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Phase>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .OrderBy(p => p.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Phase>> GetByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .Where(p => p.MilestoneId == milestoneId)
            .OrderBy(p => p.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Phase>> GetActiveByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .Where(p => p.MilestoneId == milestoneId && p.IsActive)
            .OrderBy(p => p.SequenceOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<Phase?> GetBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .FirstOrDefaultAsync(p => p.SequenceOrder == sequenceOrder, cancellationToken);
    }

    public async Task AddAsync(Phase phase, CancellationToken cancellationToken = default)
    {
        await _context.Phases.AddAsync(phase, cancellationToken);
    }

    public void Update(Phase phase)
    {
        _context.Phases.Update(phase);
    }

    public void Delete(Phase phase)
    {
        _context.Phases.Remove(phase);
    }

    public async Task<bool> ExistsByIdAsync(PhaseId id, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default)
    {
        return await _context.Phases
            .AnyAsync(p => p.SequenceOrder == sequenceOrder, cancellationToken);
    }
}
