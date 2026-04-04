using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IRequirementCapabilityDetailRepository
{
    Task<RequirementCapabilityDetail?> GetByIdAsync(RequirementCapabilityDetailId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RequirementCapabilityDetail>> GetByRequirementIdAsync(CustomDesignRequirementId requirementId, CancellationToken cancellationToken = default);
    Task<RequirementCapabilityDetail?> GetByRequirementAndCapabilityAsync(CustomDesignRequirementId requirementId, Guid capabilityId, CancellationToken cancellationToken = default);
    Task AddAsync(RequirementCapabilityDetail detail, CancellationToken cancellationToken = default);
    void Delete(RequirementCapabilityDetail detail);
    Task<bool> ExistsByIdAsync(RequirementCapabilityDetailId id, CancellationToken cancellationToken = default);
}
