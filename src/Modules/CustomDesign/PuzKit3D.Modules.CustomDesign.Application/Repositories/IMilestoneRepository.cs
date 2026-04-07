using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IMilestoneRepository
{
    Task<Milestone?> GetByIdAsync(MilestoneId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Milestone>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Milestone>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<Milestone?> GetBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default);
    Task AddAsync(Milestone milestone, CancellationToken cancellationToken = default);
    void Update(Milestone milestone);
    void Delete(Milestone milestone);
    Task<bool> ExistsByIdAsync(MilestoneId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySequenceOrderAsync(int sequenceOrder, CancellationToken cancellationToken = default);
}
