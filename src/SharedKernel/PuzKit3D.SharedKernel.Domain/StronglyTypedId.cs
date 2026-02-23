using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Domain;

public abstract class StronglyTypedId<TValue> : ValueObject
    where TValue : notnull
{
    protected StronglyTypedId(TValue value)
    {
        Value = value;
    }

    public TValue Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString() ?? string.Empty;
}
