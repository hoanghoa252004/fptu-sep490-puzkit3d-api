using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Application.Repositories;

public interface IImportServiceConfigRepository : IRepositoryBase<Domain.Entities.ImportServiceConfigs.ImportServiceConfig, ImportServiceConfigId>
{
}
