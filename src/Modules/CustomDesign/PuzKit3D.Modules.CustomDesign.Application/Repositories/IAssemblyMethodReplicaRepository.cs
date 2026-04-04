using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IAssemblyMethodReplicaRepository
{
    Task<AssemblyMethodReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AssemblyMethodReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssemblyMethodReplica>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(AssemblyMethodReplica assemblyMethod);
    void Update(AssemblyMethodReplica assemblyMethod);
    void Remove(AssemblyMethodReplica assemblyMethod);
}