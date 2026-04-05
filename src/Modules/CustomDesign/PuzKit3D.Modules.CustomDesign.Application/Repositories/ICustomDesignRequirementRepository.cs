using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICustomDesignRequirementRepository
{
    Task<ResultT<CustomDesignRequirement>> GetByIdAsync(CustomDesignRequirementId id, CancellationToken cancellationToken = default);
    Task<ResultT<CustomDesignRequirement>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequirement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequirement>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(CustomDesignRequirement requirement, CancellationToken cancellationToken = default);
    void Update(CustomDesignRequirement requirement);
    void Delete(CustomDesignRequirement requirement);
    Task<bool> ExistsByIdAsync(CustomDesignRequirementId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsDuplicateAsync(Guid topicId, Guid materialId, Guid assemblyMethodId, DifficultyLevel difficulty, IEnumerable<Guid> capabilityIds, CancellationToken cancellationToken = default);
}
