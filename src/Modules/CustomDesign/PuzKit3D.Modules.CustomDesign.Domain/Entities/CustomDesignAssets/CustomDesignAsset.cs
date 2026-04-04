using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;

public sealed class CustomDesignAsset : Entity<CustomDesignAssetId>
{
    public string Code { get; private set; } = null!;
    public CustomDesignRequirementId CustomDesignRequirementId { get; private set; }
    public int Version { get; private set; }
    public string? Sketches { get; private set; }
    public string? SketchTaskId { get; private set; }
    public string? Rough3DModel { get; private set; }
    public string? Rough3DModelTaskId { get; private set; }
    public string? Note { get; private set; }
    public bool IsFinalDesign { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private CustomDesignAsset(
        CustomDesignAssetId id,
        string code,
        CustomDesignRequirementId customDesignRequirementId,
        int version,
        string? sketches,
        string? sketchTaskId,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? note,
        bool isFinalDesign,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        CustomDesignRequirementId = customDesignRequirementId;
        Version = version;
        Sketches = sketches;
        SketchTaskId = sketchTaskId;
        Rough3DModel = rough3DModel;
        Rough3DModelTaskId = rough3DModelTaskId;
        Note = note;
        IsFinalDesign = isFinalDesign;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private CustomDesignAsset() : base()
    {
    }

    public static CustomDesignAsset Create(
        Guid id,
        string code,
        Guid customDesignRequirementId,
        int version,
        string? sketches,
        string? sketchTaskId,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? note,
        bool isFinalDesign,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new CustomDesignAsset(
            CustomDesignAssetId.From(id),
            code,
            CustomDesignRequirementId.From(customDesignRequirementId),
            version,
            sketches,
            sketchTaskId,
            rough3DModel,
            rough3DModelTaskId,
            note,
            isFinalDesign,
            createdAt,
            updatedAt);
    }

    public void Update(
        string? sketches,
        string? sketchTaskId,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? note,
        bool isFinalDesign,
        DateTime updatedAt)
    {
        Sketches = sketches;
        SketchTaskId = sketchTaskId;
        Rough3DModel = rough3DModel;
        Rough3DModelTaskId = rough3DModelTaskId;
        Note = note;
        IsFinalDesign = isFinalDesign;
        UpdatedAt = updatedAt;
    }
}
