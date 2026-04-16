using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductCapabilityDetailRepository
{
    void Add(InstockProductCapabilityDetail capabilityDetail);
    void Delete(InstockProductCapabilityDetail capabilityDetail);
    void Update(InstockProductCapabilityDetail capabilityDetail);
}
