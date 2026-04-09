using PuzKit3D.Modules.Catalog.Domain.Entities.Materials.DomainEvents;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

public class Material : AggregateRoot<MaterialId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public decimal FactorPercentage { get; private set; }
    public decimal BasePrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<TopicMaterialCapability> TopicMaterialCapabilities { get; private set; } = new List<TopicMaterialCapability>();
    public ICollection<CapabilityMaterialAssembly> CapabilityMaterialAssemblies { get; private set; } = new List<CapabilityMaterialAssembly>();

    private Material(
        MaterialId id,
        string name,
        string slug,
        decimal factorPercentage,
        decimal basePrice,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Slug = slug;
        FactorPercentage = factorPercentage;
        BasePrice = basePrice;
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
        decimal factorPercentage,
        decimal basePrice,
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
            factorPercentage,
            basePrice,
            description,
            isActive,
            timestamp);

        material.RaiseDomainEvent(new MaterialCreatedDomainEvent(
            material.Id.Value,
            material.Name,
            material.Slug,
            material.FactorPercentage,
            material.BasePrice,
            material.Description,
            material.IsActive,
            material.CreatedAt));

        return Result.Success(material);
    }

    public Result Update(string? name = null, string? slug = null, decimal? factorPercentage = null, decimal? basePrice = null, string? description = null, bool? isActive = null)
    {
        // Validate only provided fields
        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(MaterialError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(MaterialError.NameTooLong(name.Length));

            Name = name;
        }

        if (slug != null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result.Failure(MaterialError.InvalidSlug());

            if (slug.Length > 30)
                return Result.Failure(MaterialError.SlugTooLong(slug.Length));

            Slug = slug;
        }

        if (factorPercentage.HasValue)
        {
            FactorPercentage = factorPercentage.Value;
        }

        if (basePrice.HasValue)
        {
            BasePrice = basePrice.Value;
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

        RaiseDomainEvent(new MaterialUpdatedDomainEvent(
            Id.Value,
            Name,
            Slug,
            Description,
            UpdatedAt));

        return Result.Success();
    }

    public void Delete()
    {
        RaiseDomainEvent(new MaterialDeletedDomainEvent(
            Id.Value,
            DateTime.UtcNow));
    }
}
