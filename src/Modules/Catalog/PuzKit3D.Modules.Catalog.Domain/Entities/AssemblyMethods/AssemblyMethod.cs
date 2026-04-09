using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods.DomainEvents;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

public class AssemblyMethod : AggregateRoot<AssemblyMethodId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public decimal FactorPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<CapabilityMaterialAssembly> CapabilityMaterialAssemblies { get; private set; } = new List<CapabilityMaterialAssembly>();

    private AssemblyMethod(
        AssemblyMethodId id,
        string name,
        string slug,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Slug = slug;
        FactorPercentage = factorPercentage;
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
        decimal factorPercentage,
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
            factorPercentage,
            description,
            isActive,
            timestamp);

        assemblyMethod.RaiseDomainEvent(new AssemblyMethodCreatedDomainEvent(
            assemblyMethod.Id.Value,
            assemblyMethod.Name,
            assemblyMethod.Slug,
            assemblyMethod.FactorPercentage,
            assemblyMethod.Description,
            assemblyMethod.IsActive,
            assemblyMethod.CreatedAt));

        return Result.Success(assemblyMethod);
    }

    public Result Update(string? name = null, string? slug = null, decimal? factorPercentage = null, string? description = null, bool? isActive = null)
    {
        // Validate only provided fields
        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(AssemblyMethodError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(AssemblyMethodError.NameTooLong(name.Length));

            Name = name;
        }

        if (slug != null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result.Failure(AssemblyMethodError.InvalidSlug());

            if (slug.Length > 30)
                return Result.Failure(AssemblyMethodError.SlugTooLong(slug.Length));

            Slug = slug;
        }

        if (factorPercentage.HasValue)
        {
            FactorPercentage = factorPercentage.Value;
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

        RaiseDomainEvent(new AssemblyMethodUpdatedDomainEvent(
            Id.Value,
            Name,
            Slug,
            FactorPercentage,
            Description,
            UpdatedAt));

        return Result.Success();
    }

    public void Delete()
    {
        RaiseDomainEvent(new AssemblyMethodDeletedDomainEvent(
            Id.Value,
            DateTime.UtcNow));
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
