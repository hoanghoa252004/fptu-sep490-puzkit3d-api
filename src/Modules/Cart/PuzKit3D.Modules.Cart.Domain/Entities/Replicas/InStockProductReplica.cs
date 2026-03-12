using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductReplica : Entity<Guid>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? BriefDescription { get; private set; }
    public string? DetailDescription { get; private set; }
    public string DifficultLevel { get; private set; }
    public int EstimatedBuildTime { get; private set; }
    public string ThumbnailUrl { get; private set; }
    public string Slug { get; private set; }
    public string? Specification { get; private set; }
    public string PreviewAsset { get; private set; }
    public Guid TopicId { get; private set; }
    public Guid AssemblyMethod { get; private set; }
    public Guid Capability { get; private set; }
    public Guid Material { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private InStockProductReplica() : base()
    {
    }

    public static InStockProductReplica Create(
        Guid id,
        string code,
        string name,
        string? briefDescription,
        string? detailDescription,
        string difficultLevel,
        int estimatedBuildTime,
        string thumbnailUrl,
        string slug,
        string? specification,
        string previewAsset,
        Guid topicId,
        Guid assemblyMethod,
        Guid capability,
        Guid material,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockProductReplica
        {
            Id = id,
            Code = code,
            Name = name,
            BriefDescription = briefDescription,
            DetailDescription = detailDescription,
            DifficultLevel = difficultLevel,
            EstimatedBuildTime = estimatedBuildTime,
            ThumbnailUrl = thumbnailUrl,
            Slug = slug,
            Specification = specification,
            PreviewAsset = previewAsset,
            TopicId = topicId,
            AssemblyMethod = assemblyMethod,
            Capability = capability,
            Material = material,
            IsActive = isActive,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void Update(
        string code,
        string name,
        string difficultLevel,
        int estimatedBuildTime,
        string thumbnailUrl,
        string slug,
        string previewAsset,
        Guid topicId,
        Guid assemblyMethod,
        Guid capability,
        Guid material,
        bool isActive,
        DateTime updatedAt)
    {
        Code = code;
        Name = name;
        DifficultLevel = difficultLevel;
        EstimatedBuildTime = estimatedBuildTime;
        ThumbnailUrl = thumbnailUrl;
        Slug = slug;
        PreviewAsset = previewAsset;
        TopicId = topicId;
        AssemblyMethod = assemblyMethod;
        Capability = capability;
        Material = material;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
