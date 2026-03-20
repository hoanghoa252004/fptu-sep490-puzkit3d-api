using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;

public sealed class ProductReplica : Entity<Guid>
{
    public string Type { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    private ProductReplica(
        Guid id,
        string type,
        string name) : base(id)
    {
        Type = type;
        Name = name;
    }
    private ProductReplica() : base()
    {
    }
    public static ProductReplica Create(
        Guid id,
        string type,
        string name)
    {
        return new ProductReplica(
            id,
            type,
            name);
    }
}
