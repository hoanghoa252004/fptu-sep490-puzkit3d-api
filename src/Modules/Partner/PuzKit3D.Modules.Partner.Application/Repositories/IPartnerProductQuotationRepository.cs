using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductQuotationRepository : IRepositoryBase<PartnerProductQuotation, PartnerProductQuotationId>
{
    Task<IEnumerable<PartnerProductQuotation>> GetAllAsync(
        DateTime? createdAtFrom = null,
        DateTime? createdAtTo = null,
        bool ascending = false,
        CancellationToken cancellationToken = default);

    Task<PartnerProductQuotation?> GetByRequestIdAsync(PartnerProductRequestId requestId, CancellationToken cancellationToken = default);
}
