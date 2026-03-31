using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductRequestRepository : IRepositoryBase<Domain.Entities.PartnerProductRequests.PartnerProductRequest, PartnerProductRequestId>
{
}
