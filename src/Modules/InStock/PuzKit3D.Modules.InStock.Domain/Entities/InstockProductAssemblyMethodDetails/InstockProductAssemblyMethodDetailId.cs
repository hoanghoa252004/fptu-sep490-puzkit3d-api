namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductAssemblyMethodDetails;

public sealed class InstockProductAssemblyMethodDetailId : IEquatable<InstockProductAssemblyMethodDetailId>
{
    public Guid Value { get; }

    private InstockProductAssemblyMethodDetailId(Guid value)
    {
        Value = value;
    }

    public static InstockProductAssemblyMethodDetailId From(Guid value) => new(value);
    public static InstockProductAssemblyMethodDetailId Create() => new(Guid.NewGuid());

    public override bool Equals(object? obj) =>
        obj is InstockProductAssemblyMethodDetailId other && Equals(other);

    public bool Equals(InstockProductAssemblyMethodDetailId? other) =>
        other is not null && Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
