using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IPhaseRepository
{
    Task<Phase?> GetByIdAsync(PhaseId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Phase>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Phase>> GetByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Phase>> GetActiveByMilestoneIdAsync(MilestoneId milestoneId, CancellationToken cancellationToken = default);
    Task<Phase?> GetBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default);
    Task AddAsync(Phase phase, CancellationToken cancellationToken = default);
    void Update(Phase phase);
    void Delete(Phase phase);
    Task<bool> ExistsByIdAsync(PhaseId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default);
}
