using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly InStockDbContext _context;

    public ProductRepository(InStockDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> FindAsync(
        Expression<Func<Product, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Name == name, cancellationToken);
    }

    public void Add(Product entity)
    {
        _context.Products.Add(entity);
    }

    public void Update(Product entity)
    {
        _context.Products.Update(entity);
    }

    public void Delete(Product entity)
    {
        _context.Products.Remove(entity);
    }

    public void DeleteMultiple(List<Product> entities)
    {
        _context.Products.RemoveRange(entities);
    }
}
