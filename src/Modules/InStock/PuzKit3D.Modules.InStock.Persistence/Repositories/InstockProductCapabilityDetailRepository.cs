using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;
using PuzKit3D.Modules.InStock.Persistence;

namespace PuzKit3D.Modules.InStock.Persistence.Repositories;

internal sealed class InstockProductCapabilityDetailRepository : IInstockProductCapabilityDetailRepository
{
    private readonly InStockDbContext _context;

    public InstockProductCapabilityDetailRepository(InStockDbContext context)
    {
        _context = context;
    }

    public void Add(InstockProductCapabilityDetail capabilityDetail)
    {
        _context.InstockProductCapabilityDetails.Add(capabilityDetail);
    }

    public void Delete(InstockProductCapabilityDetail capabilityDetail)
    {
        _context.InstockProductCapabilityDetails.Remove(capabilityDetail);
    }

    public void Update(InstockProductCapabilityDetail capabilityDetail)
    {
        _context.InstockProductCapabilityDetails.Update(capabilityDetail);
    }
}
