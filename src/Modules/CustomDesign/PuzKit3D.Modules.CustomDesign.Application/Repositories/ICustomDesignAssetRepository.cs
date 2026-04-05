using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICustomDesignAssetRepository
{
    Task<CustomDesignAsset?> GetByIdAsync(CustomDesignAssetId id, CancellationToken cancellationToken = default);
    Task<CustomDesignAsset?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetByRequirementIdAsync(CustomDesignRequirementId requirementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetFinalDesignsByRequirementIdAsync(CustomDesignRequirementId requirementId, CancellationToken cancellationToken = default);
    Task AddAsync(CustomDesignAsset asset, CancellationToken cancellationToken = default);
    void Update(CustomDesignAsset asset);
    void Delete(CustomDesignAsset asset);
    Task<bool> ExistsByIdAsync(CustomDesignAssetId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
