using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Application.Repositories;

public interface ICartRepository : IRepositoryBase<Domain.Entities.Carts.Cart, CartId>
{
    Task<Domain.Entities.Carts.Cart?> GetByUserIdAndCartTypeAsync(
        Guid userId, 
        string cartType, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Domain.Entities.Carts.Cart>> GetByUserIdAsync(
        Guid userId, 
        CancellationToken cancellationToken = default);
}
