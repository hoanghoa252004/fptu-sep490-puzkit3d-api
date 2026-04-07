using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerRepository : IRepositoryBase<Domain.Entities.Partners.Partner, PartnerId>
{
    Task<Domain.Entities.Partners.Partner?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Partners.Partner?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Partners.Partner?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Domain.Entities.Partners.Partner?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.Partners.Partner>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default);
}
