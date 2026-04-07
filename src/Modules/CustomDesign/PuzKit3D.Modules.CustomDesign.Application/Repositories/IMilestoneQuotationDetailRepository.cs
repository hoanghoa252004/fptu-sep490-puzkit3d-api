using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IMilestoneQuotationDetailRepository
{
    Task<MilestoneQuotationDetail?> GetByIdAsync(MilestoneQuotationDetailId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MilestoneQuotationDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<MilestoneQuotationDetail>> GetByMilestoneQuotationIdAsync(MilestoneQuotationId milestoneQuotationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MilestoneQuotationDetail>> GetByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default);
    Task AddAsync(MilestoneQuotationDetail detail, CancellationToken cancellationToken = default);
    void Update(MilestoneQuotationDetail detail);
    void Delete(MilestoneQuotationDetail detail);
    Task<bool> ExistsByIdAsync(MilestoneQuotationDetailId id, CancellationToken cancellationToken = default);
}
