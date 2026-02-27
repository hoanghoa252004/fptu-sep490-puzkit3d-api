using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface IMaterialRepository : IRepositoryBase<Material, MaterialId>
{
    Task<Material?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
