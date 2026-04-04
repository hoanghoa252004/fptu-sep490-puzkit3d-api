using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Application.Repositories;

public interface ITopicReplicaRepository
{
    Task<TopicReplica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TopicReplica?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<TopicReplica>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(TopicReplica topic);
    void Update(TopicReplica topic);
    void Remove(TopicReplica topic);
}

