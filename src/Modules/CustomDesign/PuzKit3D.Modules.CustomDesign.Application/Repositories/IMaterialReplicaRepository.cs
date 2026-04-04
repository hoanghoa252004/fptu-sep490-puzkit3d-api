using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface IMaterialReplicaRepository
{
    Task<MaterialReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MaterialReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialReplica>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(MaterialReplica material);
    void Update(MaterialReplica material);
    void Remove(MaterialReplica material);
}
