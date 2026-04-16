using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class InStockProductReplica : Entity<Guid>
{
    public string Code { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int TotalPieceCount { get; private set; }
    public string DifficultLevel { get; private set; } = null!;
    public int EstimatedBuildTime { get; private set; }
    public string ThumbnailUrl { get; private set; } = null!;
    public string PreviewAsset { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid TopicId { get; private set; }
    public Guid MaterialId { get; private set; }
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
        string? description,
        string difficultLevel,
        int estimatedBuildTime,
        int totalPieceCount,
        string thumbnailUrl,
        string slug,
        string previewAsset,
        Guid topicId,
        Guid materialId,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new InStockProductReplica
        {
            Id = id,
            Code = code,
            Name = name,
            Description = description,
            DifficultLevel = difficultLevel,
            EstimatedBuildTime = estimatedBuildTime,
            TotalPieceCount = totalPieceCount,
            ThumbnailUrl = thumbnailUrl,
            Slug = slug,
            PreviewAsset = previewAsset,
            TopicId = topicId,
            MaterialId = materialId,
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
        Guid materialId,
        string? description,
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
        MaterialId = materialId;
        IsActive = isActive;
        UpdatedAt = updatedAt;
        Description = description;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
