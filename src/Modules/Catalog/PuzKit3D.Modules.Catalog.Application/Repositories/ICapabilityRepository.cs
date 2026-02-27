using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface ICapabilityRepository : IRepositoryBase<Capability, CapabilityId>
{
    Task<Capability?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
}
