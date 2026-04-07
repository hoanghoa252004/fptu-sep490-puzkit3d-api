using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class ProposalRepository : IProposalRepository
{
    private readonly CustomDesignDbContext _context;

    public ProposalRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<Proposal?> GetByIdAsync(ProposalId id, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Proposal?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetByRequestIdAsync(CustomDesignRequestId requestId, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .Where(p => p.RequestId == requestId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Proposal>> GetByStatusAsync(ProposalStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .Where(p => p.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Proposal proposal, CancellationToken cancellationToken = default)
    {
        await _context.Proposals.AddAsync(proposal, cancellationToken);
    }

    public void Update(Proposal proposal)
    {
        _context.Proposals.Update(proposal);
    }

    public void Delete(Proposal proposal)
    {
        _context.Proposals.Remove(proposal);
    }

    public async Task<bool> ExistsByIdAsync(ProposalId id, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Proposals
            .AnyAsync(p => p.Code == code, cancellationToken);
    }
}
