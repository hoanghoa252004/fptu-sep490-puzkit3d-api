using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

public class Material : AggregateRoot<MaterialId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Material(
        MaterialId id,
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

    private Material() : base()
    {
    }

    public static ResultT<Material> Create(
        string name,
        string slug,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Material>(MaterialError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Material>(MaterialError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Material>(MaterialError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<Material>(MaterialError.SlugTooLong(slug.Length));

        var materialId = MaterialId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var material = new Material(
            materialId,
            name,
            slug,
            description,
            isActive,
            timestamp);

        return Result.Success(material);
    }

    public Result Update(string name, string slug, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(MaterialError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(MaterialError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(MaterialError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(MaterialError.SlugTooLong(slug.Length));

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
