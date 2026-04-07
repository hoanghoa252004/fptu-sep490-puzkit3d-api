using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Repositories;

internal sealed class ProductQuotationRepository : IProductQuotationRepository
{
    private readonly CustomDesignDbContext _context;

    public ProductQuotationRepository(CustomDesignDbContext context)
    {
        _context = context;
    }

    public async Task<ProductQuotation?> GetByIdAsync(ProductQuotationId id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .FirstOrDefaultAsync(pq => pq.Id == id, cancellationToken);
    }

    public async Task<ProductQuotation?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .FirstOrDefaultAsync(pq => pq.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<ProductQuotation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductQuotation>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .Where(pq => pq.ProposalId == proposalId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductQuotation>> GetByMaterialIdAsync(Guid materialId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .Where(pq => pq.MaterialId == materialId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProductQuotation productQuotation, CancellationToken cancellationToken = default)
    {
        await _context.ProductQuotations.AddAsync(productQuotation, cancellationToken);
    }

    public void Update(ProductQuotation productQuotation)
    {
        _context.ProductQuotations.Update(productQuotation);
    }

    public void Delete(ProductQuotation productQuotation)
    {
        _context.ProductQuotations.Remove(productQuotation);
    }

    public async Task<bool> ExistsByIdAsync(ProductQuotationId id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .AnyAsync(pq => pq.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.ProductQuotations
            .AnyAsync(pq => pq.Code == code, cancellationToken);
    }
}
