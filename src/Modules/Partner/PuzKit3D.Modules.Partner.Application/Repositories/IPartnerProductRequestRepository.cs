using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductRequestRepository : IRepositoryBase<Domain.Entities.PartnerProductRequests.PartnerProductRequest, PartnerProductRequestId>
{
    Task<PartnerProductRequest?> GetDetailByIdAsync(
        PartnerProductRequestId id, 
        CancellationToken cancellationToken);
    Task<IEnumerable<PartnerProductRequest>> GetAllAsync(
        int? status, 
        string? searchTerm = null, 
        bool ascending = false, 
        CancellationToken cancellationToken = default);
    Task<IEnumerable<PartnerProductRequest>> GetAllAsync(
        IEnumerable<PartnerProductRequestStatus> statuses,
        string? searchTerm,
        CancellationToken cancellationToken);
}
