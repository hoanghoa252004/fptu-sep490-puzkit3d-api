using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public sealed class CustomDesignRequest : Entity<CustomDesignRequestId>
{
    public string Code { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public CustomDesignRequirementId CustomDesignRequirementId { get; private set; }
    public decimal DesiredLengthMm { get; private set; }
    public decimal DesiredWidthMm { get; private set; }
    public decimal DesiredHeightMm { get; private set; }
    public string? Sketches { get; private set; }
    public string? CustomerPrompt { get; private set; }
    public DateTime DesiredDeliveryDate { get; private set; }
    public int DesiredQuantity { get; private set; }
    public decimal TargetBudget { get; private set; }
    public int UsedSupportConceptDesignTime { get; private set; }
    public CustomDesignRequestStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private CustomDesignRequest(
        CustomDesignRequestId id,
        string code,
        string title,
        string? description,
        CustomDesignRequirementId customDesignRequirementId,
        decimal desiredLengthMm,
        decimal desiredWidthMm,
        decimal desiredHeightMm,
        string? sketches,
        string? customerPrompt,
        DateTime desiredDeliveryDate,
        int desiredQuantity,
        decimal targetBudget,
        int usedSupportConceptDesignTime,
        CustomDesignRequestStatus status,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        Title = title;
        Description = description;
        CustomDesignRequirementId = customDesignRequirementId;
        DesiredLengthMm = desiredLengthMm;
        DesiredWidthMm = desiredWidthMm;
        DesiredHeightMm = desiredHeightMm;
        Sketches = sketches;
        CustomerPrompt = customerPrompt;
        DesiredDeliveryDate = desiredDeliveryDate;
        DesiredQuantity = desiredQuantity;
        TargetBudget = targetBudget;
        UsedSupportConceptDesignTime = usedSupportConceptDesignTime;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private CustomDesignRequest() : base()
    {
    }

    public static CustomDesignRequest Create(
        Guid id,
        string code,
        string title,
        string? description,
        Guid customDesignRequirementId,
        decimal desiredLengthMm,
        decimal desiredWidthMm,
        decimal desiredHeightMm,
        string? sketches,
        string? customerPrompt,
        DateTime desiredDeliveryDate,
        int desiredQuantity,
        decimal targetBudget,
        int usedSupportConceptDesignTime,
        CustomDesignRequestStatus status,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new CustomDesignRequest(
            CustomDesignRequestId.From(id),
            code,
            title,
            description,
            CustomDesignRequirementId.From(customDesignRequirementId),
            desiredLengthMm,
            desiredWidthMm,
            desiredHeightMm,
            sketches,
            customerPrompt,
            desiredDeliveryDate,
            desiredQuantity,
            targetBudget,
            usedSupportConceptDesignTime,
            status,
            createdAt,
            updatedAt);
    }

    public void Update(
        string title,
        string? description,
        decimal desiredLengthMm,
        decimal desiredWidthMm,
        decimal desiredHeightMm,
        string? sketches,
        string? customerPrompt,
        DateTime desiredDeliveryDate,
        int desiredQuantity,
        decimal targetBudget,
        int usedSupportConceptDesignTime,
        CustomDesignRequestStatus status,
        DateTime updatedAt)
    {
        Title = title;
        Description = description;
        DesiredLengthMm = desiredLengthMm;
        DesiredWidthMm = desiredWidthMm;
        DesiredHeightMm = desiredHeightMm;
        Sketches = sketches;
        CustomerPrompt = customerPrompt;
        DesiredDeliveryDate = desiredDeliveryDate;
        DesiredQuantity = desiredQuantity;
        TargetBudget = targetBudget;
        UsedSupportConceptDesignTime = usedSupportConceptDesignTime;
        Status = status;
        UpdatedAt = updatedAt;
    }
}
