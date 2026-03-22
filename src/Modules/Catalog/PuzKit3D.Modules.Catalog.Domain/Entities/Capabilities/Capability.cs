using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities.DomainEvents;
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

        capability.RaiseDomainEvent(new CapabilityCreatedDomainEvent(
            capability.Id.Value,
            capability.Name,
            capability.Slug,
            capability.Description,
            capability.IsActive,
            capability.CreatedAt));

        return Result.Success(capability);
    }

    public Result Update(string? name = null, string? slug = null, string? description = null, bool? isActive = null)
    {
        // Validate only provided fields
        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(CapabilityError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(CapabilityError.NameTooLong(name.Length));

            Name = name;
        }

        if (slug != null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result.Failure(CapabilityError.InvalidSlug());

            if (slug.Length > 30)
                return Result.Failure(CapabilityError.SlugTooLong(slug.Length));

            Slug = slug;
        }

        if (description != null)
        {
            Description = description;
        }

        if (isActive.HasValue && isActive.Value != IsActive)
        {
            IsActive = isActive.Value;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new CapabilityUpdatedDomainEvent(
            Id.Value,
            Name,
            Slug,
            Description,
            UpdatedAt));

        return Result.Success();
    }

    public void Delete()
    {
        RaiseDomainEvent(new CapabilityDeletedDomainEvent(
            Id.Value,
            DateTime.UtcNow));
    }
}
