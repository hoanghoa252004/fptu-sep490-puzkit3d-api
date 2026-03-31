using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductRequestDetailRepository : IRepositoryBase<Domain.Entities.PartnerProductRequestDetails.PartnerProductRequestDetail, PartnerProductRequestDetailId>
{
    Task<IEnumerable<Domain.Entities.PartnerProductRequestDetails.PartnerProductRequestDetail>> FindByRequestIdAsync(
        PartnerProductRequestId requestId, 
        CancellationToken cancellationToken = default);
}
