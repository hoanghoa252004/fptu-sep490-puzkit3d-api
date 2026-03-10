using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductPriceDetailRepository : IRepositoryBase<InstockProductPriceDetail, InstockProductPriceDetailId>
{
    Task<InstockProductPriceDetail?> GetByPriceAndVariantAsync(
        Guid priceId, 
        Guid variantId, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<InstockProductPriceDetail>> GetAllByProductVariantIdAsync(
        InstockProductVariantId variantId,
        CancellationToken cancellationToken = default);
}
