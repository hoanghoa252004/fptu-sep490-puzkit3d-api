using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICustomDesignAssetRepository
{
    Task<ResultT<CustomDesignAsset>> GetByIdAsync(CustomDesignAssetId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetByRequestIdAsync(CustomDesignRequestId requestId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignAsset>> GetFinalDesignsByRequestIdAsync(CustomDesignRequestId requestId, CancellationToken cancellationToken = default);
    Task AddAsync(CustomDesignAsset asset, CancellationToken cancellationToken = default);
    void Update(CustomDesignAsset asset);
    void Delete(CustomDesignAsset asset);
    Task<bool> ExistsByIdAsync(CustomDesignAssetId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
