using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>
    where TKey : notnull
{
    protected AggregateRoot(TKey id) : base(id)
    {
    }

    protected AggregateRoot() : base()
    {
    }
}
