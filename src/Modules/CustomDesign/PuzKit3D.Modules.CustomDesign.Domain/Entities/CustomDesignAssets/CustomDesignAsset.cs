using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;

public sealed class CustomDesignAsset : Entity<CustomDesignAssetId>
{
    public string Code { get; private set; } = null!;
    public CustomDesignRequestId CustomDesignRequestId { get; private set; }
    public int Version { get; private set; }
    public CustomDesignAssetStatus Status { get; private set; }
    public string? MultiviewImages { get; private set; }
    public string? CompositeMultiviewImage { get; private set; }
    public string? Rough3DModel { get; private set; }
    public string? Rough3DModelTaskId { get; private set; }
    public string? CustomerPrompt { get; private set; }
    public string? NormalizePrompt { get; private set; }
    public bool IsNeedSupport { get; private set; }
    public bool IsFinalDesign { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private CustomDesignAsset(
        CustomDesignAssetId id,
        string code,
        CustomDesignRequestId customDesignRequestId,
        int version,
        CustomDesignAssetStatus status,
        string? multiviewImages,
        string? compositeMultiviewImage,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? customerPrompt,
        string? normalizePrompt,
        bool isNeedSupport,
        bool isFinalDesign,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        CustomDesignRequestId = customDesignRequestId;
        Version = version;
        Status = status;
        MultiviewImages = multiviewImages;
        CompositeMultiviewImage = compositeMultiviewImage;
        Rough3DModel = rough3DModel;
        Rough3DModelTaskId = rough3DModelTaskId;
        CustomerPrompt = customerPrompt;
        NormalizePrompt = normalizePrompt;
        IsNeedSupport = isNeedSupport;
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
        Guid customDesignRequestId,
        int version,
        string? multiviewImages,
        string? compositeMultiviewImage,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? customerPrompt,
        string? normalizePrompt,
        bool isNeedSupport,
        bool isFinalDesign,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new CustomDesignAsset(
            CustomDesignAssetId.From(id),
            code,
            CustomDesignRequestId.From(customDesignRequestId),
            version,
            CustomDesignAssetStatus.ImageProcessing,
            multiviewImages,
            compositeMultiviewImage,
            rough3DModel,
            rough3DModelTaskId,
            customerPrompt,
            normalizePrompt,
            isNeedSupport,
            isFinalDesign,
            createdAt,
            updatedAt);
    }

    public void Update(
        string? multiviewImages,
        string? compositeMultiviewImage,
        string? rough3DModel,
        string? rough3DModelTaskId,
        string? customerPrompt,
        string? normalizePrompt,
        bool isNeedSupport,
        bool isFinalDesign,
        DateTime updatedAt)
    {
        MultiviewImages = multiviewImages;
        CompositeMultiviewImage = compositeMultiviewImage;
        Rough3DModel = rough3DModel;
        Rough3DModelTaskId = rough3DModelTaskId;
        CustomerPrompt = customerPrompt;
        NormalizePrompt = normalizePrompt;
        IsNeedSupport = isNeedSupport;
        IsFinalDesign = isFinalDesign;
        UpdatedAt = updatedAt;
    }

    public void UpdateStatus(CustomDesignAssetStatus status, DateTime updatedAt)
    {
        Status = status;
        UpdatedAt = updatedAt;
    }
}
