using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class MilestoneQuotationDetailRepository : IMilestoneQuotationDetailRepository
{
    private readonly CustomDesignDbContext _context;

    public MilestoneQuotationDetailRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<MilestoneQuotationDetail?> GetByIdAsync(MilestoneQuotationDetailId id, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotationDetails
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<MilestoneQuotationDetail>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotationDetails
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MilestoneQuotationDetail>> GetByMilestoneQuotationIdAsync(MilestoneQuotationId milestoneQuotationId, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotationDetails
            .Where(d => d.MilestoneQuotationId == milestoneQuotationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MilestoneQuotationDetail>> GetByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotationDetails
            .Where(d => d.MilestoneId == milestoneId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(MilestoneQuotationDetail detail, CancellationToken cancellationToken = default)
    {
        await _context.MilestoneQuotationDetails.AddAsync(detail, cancellationToken);
    }

    public void Update(MilestoneQuotationDetail detail)
    {
        _context.MilestoneQuotationDetails.Update(detail);
    }

    public void Delete(MilestoneQuotationDetail detail)
    {
        _context.MilestoneQuotationDetails.Remove(detail);
    }

    public async Task<bool> ExistsByIdAsync(MilestoneQuotationDetailId id, CancellationToken cancellationToken = default)
    {
        return await _context.MilestoneQuotationDetails
            .AnyAsync(d => d.Id == id, cancellationToken);
    }
}
