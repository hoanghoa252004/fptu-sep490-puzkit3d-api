using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IProductRepository : IRepositoryBase<Product, ProductId>
{
    Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
