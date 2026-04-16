using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.SharedKernel.Domain;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface IFormulaRepository : IRepositoryBase<Formula, FormulaId>
{
}
