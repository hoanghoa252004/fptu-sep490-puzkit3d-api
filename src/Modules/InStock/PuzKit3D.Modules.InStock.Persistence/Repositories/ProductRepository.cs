using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.Modules.InStock.Domain.Repositories;
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

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Name == name, cancellationToken);
    }

    public Product FindById(ProductId id, params Expression<Func<Product, object>>[] includeProperties)
    {
        var query = _context.Products.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(p => p.Id.Equals(id))!;
    }

    public Product FindSingle(Expression<Func<Product, bool>> predicate, params Expression<Func<Product, object>>[] includeProperties)
    {
        var query = _context.Products.AsQueryable();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.FirstOrDefault(predicate)!;
    }

    public IQueryable<Product> FindAll(Expression<Func<Product, bool>>? predicate, params Expression<Func<Product, object>>[] includeProperties)
    {
        var query = _context.Products.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query;
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
