using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductAssemblyMethodDetails;

namespace PuzKit3D.Modules.InStock.Application.Repositories;

public interface IInstockProductAssemblyMethodDetailRepository
{
    void Add(InstockProductAssemblyMethodDetail assemblyMethodDetail);
    void Delete(InstockProductAssemblyMethodDetail assemblyMethodDetail);
    void Update(InstockProductAssemblyMethodDetail assemblyMethodDetail);
}
