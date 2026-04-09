using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.SharedKernel.Domain;
using System.Linq.Expressions;

namespace PuzKit3D.Modules.Catalog.Application.Repositories;

public interface IDriveRepository : IRepositoryBase<Drive, DriveId>
{
}
