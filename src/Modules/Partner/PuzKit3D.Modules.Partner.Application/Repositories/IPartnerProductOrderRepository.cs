using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductOrderRepository : IRepositoryBase<PartnerProductOrder, PartnerProductOrderId>
{
    Task<IEnumerable<PartnerProductOrder>> GetAllAsync(
        string? status = null,
        DateTime? createdAtFrom = null,
        DateTime? createdAtTo = null,
        bool ascending = false,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PartnerProductOrder>> GetByCustomerIdAsync(
        Guid customerId,
        string? status = null,
        CancellationToken cancellationToken = default);
    Task<PartnerProductOrder?> GetByQuotationIdAsync(
        PartnerProductQuotationId quotationId,
        CancellationToken cancellationToken = default);

    Task<PartnerProductOrder?> GetByIdWithDetailsAsync(
        PartnerProductOrderId id,
        CancellationToken cancellationToken = default);
}
