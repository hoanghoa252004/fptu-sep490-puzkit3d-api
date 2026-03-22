using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerRepository : IRepositoryBase<Domain.Entities.Partners.Partner, PartnerId>
{
    Task<Domain.Entities.Partners.Partner?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
