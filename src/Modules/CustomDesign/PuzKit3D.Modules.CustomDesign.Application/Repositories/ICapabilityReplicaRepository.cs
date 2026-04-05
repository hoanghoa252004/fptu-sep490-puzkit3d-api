using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ICapabilityReplicaRepository
{
    Task<CapabilityReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CapabilityReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<CapabilityReplica>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(CapabilityReplica capability);
    void Update(CapabilityReplica capability);
    void Remove(CapabilityReplica capability);
}

