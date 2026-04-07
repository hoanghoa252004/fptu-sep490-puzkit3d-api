using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductRepository : IRepositoryBase<PartnerProduct, PartnerProductId>
{
    Task<PartnerProduct?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<PartnerProduct>> FindByPartnerIdAsync(Guid partnerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PartnerProduct>> GetAllAsync(
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        Guid? partnerId,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<PartnerProduct>> GetAllByPartnerIdAsync(
        PartnerId id,
        bool isStaffOrManager,
        string? searchTerm,
        bool ascending,
        CancellationToken cancellationToken = default);
}
