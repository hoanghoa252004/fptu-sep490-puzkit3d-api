using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductAssemblyMethodDetails;
using PuzKit3D.Modules.InStock.Persistence;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductAssemblyMethodDetailRepository : IInstockProductAssemblyMethodDetailRepository
{
    private readonly InStockDbContext _context;

    public InstockProductAssemblyMethodDetailRepository(InStockDbContext context)
    {
        _context = context;
    }

    public void Add(InstockProductAssemblyMethodDetail assemblyMethodDetail)
    {
        _context.InstockProductAssemblyMethodDetails.Add(assemblyMethodDetail);
    }

    public void Delete(InstockProductAssemblyMethodDetail assemblyMethodDetail)
    {
        _context.InstockProductAssemblyMethodDetails.Remove(assemblyMethodDetail);
    }

    public void Update(InstockProductAssemblyMethodDetail assemblyMethodDetail)
    {
        _context.InstockProductAssemblyMethodDetails.Update(assemblyMethodDetail);
    }
}
