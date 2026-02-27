using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

public class AssemblyMethod : AggregateRoot<AssemblyMethodId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private AssemblyMethod(
        AssemblyMethodId id,
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

    private AssemblyMethod() : base()
    {
    }

    public static ResultT<AssemblyMethod> Create(
        string name,
        string slug,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<AssemblyMethod>(AssemblyMethodError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<AssemblyMethod>(AssemblyMethodError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<AssemblyMethod>(AssemblyMethodError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<AssemblyMethod>(AssemblyMethodError.SlugTooLong(slug.Length));

        var assemblyMethodId = AssemblyMethodId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var assemblyMethod = new AssemblyMethod(
            assemblyMethodId,
            name,
            slug,
            description,
            isActive,
            timestamp);

        return Result.Success(assemblyMethod);
    }

    public Result Update(string name, string slug, string? description, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(AssemblyMethodError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(AssemblyMethodError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(AssemblyMethodError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(AssemblyMethodError.SlugTooLong(slug.Length));

        Name = name;
        Slug = slug;
        Description = description;
        IsActive = isActive;
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
