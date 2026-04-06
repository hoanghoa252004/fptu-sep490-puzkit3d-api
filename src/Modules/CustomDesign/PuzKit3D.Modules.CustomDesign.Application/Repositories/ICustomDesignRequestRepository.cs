using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICustomDesignRequestRepository
{
    Task<ResultT<CustomDesignRequest>> GetByIdAsync(CustomDesignRequestId id, CancellationToken cancellationToken = default);
    Task<ResultT<CustomDesignRequest>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetByRequirementIdAsync(CustomDesignRequirementId requirementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomDesignRequest>> GetByStatusAsync(CustomDesignRequestStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(CustomDesignRequest request, CancellationToken cancellationToken = default);
    void Update(CustomDesignRequest request);
    void Delete(CustomDesignRequest request);
    Task<bool> ExistsByIdAsync(CustomDesignRequestId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}



