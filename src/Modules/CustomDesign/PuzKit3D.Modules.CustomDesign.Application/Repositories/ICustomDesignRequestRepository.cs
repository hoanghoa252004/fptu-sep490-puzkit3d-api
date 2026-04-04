using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICustomDesignRequestRepository
{
    Task<CustomDesignRequest?> GetByIdAsync(CustomDesignRequestId id, CancellationToken cancellationToken = default);
    Task<CustomDesignRequest?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetByRequirementIdAsync(CustomDesignRequirementId requirementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetByStatusAsync(CustomDesignRequestStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(CustomDesignRequest request, CancellationToken cancellationToken = default);
    void Update(CustomDesignRequest request);
    void Delete(CustomDesignRequest request);
    Task<bool> ExistsByIdAsync(CustomDesignRequestId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}
