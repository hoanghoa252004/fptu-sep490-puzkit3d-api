using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class FormulaRepository : IFormulaRepository
{
    private readonly CatalogDbContext _context;

    public FormulaRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Formula?> GetByIdAsync(
        FormulaId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Formulas
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Formula>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Formulas
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Formula>> FindAsync(
        Expression<Func<Formula, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Formulas
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(Formula entity)
    {
        _context.Formulas.Add(entity);
    }

    public void Update(Formula entity)
    {
        _context.Formulas.Update(entity);
    }

    public void Delete(Formula entity)
    {
        _context.Formulas.Remove(entity);
    }

    public void DeleteMultiple(List<Formula> entities)
    {
        _context.Formulas.RemoveRange(entities);
    }
}
