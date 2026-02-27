using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

public class Capability : AggregateRoot<CapabilityId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Capability(
        CapabilityId id,
        string name,
        string slug,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Slug = slug;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Capability() : base()
    {
    }

    public static ResultT<Capability> Create(
        string name,
        string slug,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Capability>(CapabilityError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Capability>(CapabilityError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Capability>(CapabilityError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<Capability>(CapabilityError.SlugTooLong(slug.Length));

        var capabilityId = CapabilityId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var capability = new Capability(
            capabilityId,
            name,
            slug,
            description,
            isActive,
            timestamp);

        return Result.Success(capability);
    }

    public Result Update(string name, string slug, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(CapabilityError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(CapabilityError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(CapabilityError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(CapabilityError.SlugTooLong(slug.Length));

        Name = name;
        Slug = slug;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
