using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IMilestoneQuotationRepository
{
    Task<MilestoneQuotation?> GetByIdAsync(MilestoneQuotationId id, CancellationToken cancellationToken = default);
    Task<MilestoneQuotation?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<MilestoneQuotation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MilestoneQuotation>> GetByProposalIdAsync(ProposalId proposalId, CancellationToken cancellationToken = default);
    Task AddAsync(MilestoneQuotation milestoneQuotation, CancellationToken cancellationToken = default);
    void Update(MilestoneQuotation milestoneQuotation);
    void Delete(MilestoneQuotation milestoneQuotation);
    Task<bool> ExistsByIdAsync(MilestoneQuotationId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
