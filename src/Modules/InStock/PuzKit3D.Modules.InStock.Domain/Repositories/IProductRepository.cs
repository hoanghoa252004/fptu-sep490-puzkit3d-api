using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Repositories;

public interface IProductRepository : IRepositoryBase<Entities.Products.Product, Entities.Products.ProductId>
{
    Task<Entities.Products.Product?> GetByIdAsync(Entities.Products.ProductId id, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
