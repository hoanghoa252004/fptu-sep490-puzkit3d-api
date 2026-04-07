using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class WorkflowRepository : IWorkflowRepository
{
    private readonly CustomDesignDbContext _context;

    public WorkflowRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<Workflow?> GetByIdAsync(WorkflowId id, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Workflow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Workflow>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Where(w => w.ProposalId == proposalId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Workflow>> GetByPhaseIdAsync(PhaseId phaseId, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Where(w => w.PhaseId == phaseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Workflow>> GetByStatusAsync(WorkflowStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .Where(w => w.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<Workflow?> GetByProposalAndPhaseAsync(ProposalId proposalId, PhaseId phaseId, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .FirstOrDefaultAsync(w => w.ProposalId == proposalId && w.PhaseId == phaseId, cancellationToken);
    }

    public async Task AddAsync(Workflow workflow, CancellationToken cancellationToken = default)
    {
        await _context.Workflows.AddAsync(workflow, cancellationToken);
    }

    public void Update(Workflow workflow)
    {
        _context.Workflows.Update(workflow);
    }

    public void Delete(Workflow workflow)
    {
        _context.Workflows.Remove(workflow);
    }

    public async Task<bool> ExistsByIdAsync(WorkflowId id, CancellationToken cancellationToken = default)
    {
        return await _context.Workflows
            .AnyAsync(w => w.Id == id, cancellationToken);
    }
}
