using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IPartnerProductOrderDetailRepository : IRepositoryBase<PartnerProductOrderDetail, PartnerProductOrderDetailId>
{
    Task<IEnumerable<PartnerProductOrderDetail>> FindByOrderIdAsync(
        PartnerProductOrderId orderId,
        CancellationToken cancellationToken = default);
}