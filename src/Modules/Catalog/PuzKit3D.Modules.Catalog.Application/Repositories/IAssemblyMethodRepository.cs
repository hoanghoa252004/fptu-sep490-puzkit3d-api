using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface IAssemblyMethodRepository : IRepositoryBase<AssemblyMethod, AssemblyMethodId>
{
    Task<AssemblyMethod?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
