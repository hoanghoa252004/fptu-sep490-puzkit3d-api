using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.Modules.Cart.Application.Repositories;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Cart.Persistence.Repositories;

internal sealed class CartRepository : ICartRepository
{
    private readonly CartDbContext _context;

    public CartRepository(CartDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Carts.Cart?> GetByIdAsync(
        CartId id, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Carts.Cart>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Carts.Cart>> FindAsync(
        Expression<Func<Domain.Entities.Carts.Cart, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Carts.Cart?> GetByUserIdAndCartTypeAsync(
        Guid userId, 
        CartTypeId cartTypeId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.CartTypeId == cartTypeId, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Carts.Cart>> GetByUserIdAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .Where(c => c.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public void Add(Domain.Entities.Carts.Cart entity)
    {
        _context.Carts.Add(entity);
    }

    public void Update(Domain.Entities.Carts.Cart entity)
    {
        _context.Carts.Update(entity);
    }

    public void Delete(Domain.Entities.Carts.Cart entity)
    {
        _context.Carts.Remove(entity);
    }

    public void DeleteMultiple(List<Domain.Entities.Carts.Cart> entities)
    {
        _context.Carts.RemoveRange(entities);
    }
}
