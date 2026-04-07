using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class MilestoneQuotationRepository : IMilestoneQuotationRepository
{
    private readonly CustomDesignDbContext _context;

    public MilestoneQuotationRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<MilestoneQuotation?> GetByIdAsync(MilestoneQuotationId id, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .FirstOrDefaultAsync(mq => mq.Id == id, cancellationToken);
    }

    public async Task<MilestoneQuotation?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .FirstOrDefaultAsync(mq => mq.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<MilestoneQuotation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MilestoneQuotation>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .Where(mq => mq.ProposalId == proposalId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(MilestoneQuotation milestoneQuotation, CancellationToken cancellationToken = default)
    {
        await _context.MilestoneQuotations.AddAsync(milestoneQuotation, cancellationToken);
    }

    public void Update(MilestoneQuotation milestoneQuotation)
    {
        _context.MilestoneQuotations.Update(milestoneQuotation);
    }

    public void Delete(MilestoneQuotation milestoneQuotation)
    {
        _context.MilestoneQuotations.Remove(milestoneQuotation);
    }

    public async Task<bool> ExistsByIdAsync(MilestoneQuotationId id, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .AnyAsync(mq => mq.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotations
            .AnyAsync(mq => mq.Code == code, cancellationToken);
    }
}
