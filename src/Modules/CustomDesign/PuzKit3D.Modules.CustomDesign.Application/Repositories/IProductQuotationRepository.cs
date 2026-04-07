using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IProductQuotationRepository
{
    Task<ProductQuotation?> GetByIdAsync(ProductQuotationId id, CancellationToken cancellationToken = default);
    Task<ProductQuotation?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductQuotation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductQuotation>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductQuotation>> GetByMaterialIdAsync(Guid materialId, CancellationToken cancellationToken = default);
    Task AddAsync(ProductQuotation productQuotation, CancellationToken cancellationToken = default);
    void Update(ProductQuotation productQuotation);
    void Delete(ProductQuotation productQuotation);
    Task<bool> ExistsByIdAsync(ProductQuotationId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
