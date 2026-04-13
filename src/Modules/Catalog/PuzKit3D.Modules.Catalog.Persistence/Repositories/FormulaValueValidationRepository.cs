using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Persistence.Repositories;

internal sealed class FormulaValueValidationRepository : IFormulaValueValidationRepository
{
    private readonly CatalogDbContext _context;

    public FormulaValueValidationRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<FormulaValueValidation?> GetByIdAsync(
        FormulaValueValidationId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.FormulaValueValidations
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FormulaValueValidation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.FormulaValueValidations
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FormulaValueValidation>> FindAsync(
        Expression<Func<FormulaValueValidation, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _context.FormulaValueValidations
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Add(FormulaValueValidation entity)
    {
        _context.FormulaValueValidations.Add(entity);
    }

    public void Update(FormulaValueValidation entity)
    {
        _context.FormulaValueValidations.Update(entity);
    }

    public void Delete(FormulaValueValidation entity)
    {
        _context.FormulaValueValidations.Remove(entity);
    }

    public void DeleteMultiple(List<FormulaValueValidation> entities)
    {
        _context.FormulaValueValidations.RemoveRange(entities);
    }
}
