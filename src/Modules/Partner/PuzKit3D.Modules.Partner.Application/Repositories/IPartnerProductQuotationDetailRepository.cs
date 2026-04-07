using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductQuotationDetailRepository : IRepositoryBase<PartnerProductQuotationDetail, PartnerProductQuotationDetailId>
{
    Task<IEnumerable<PartnerProductQuotationDetail>> FindByQuotationIdAsync(
        PartnerProductQuotationId quotationId,
        CancellationToken cancellationToken = default);
}
