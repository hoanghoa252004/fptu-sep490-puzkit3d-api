using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests.DomainEvents;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;

public sealed class CustomDesignRequest : AggregateRoot<CustomDesignRequestId>
{
    public string Code { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public CustomDesignRequirementId CustomDesignRequirementId { get; private set; } = null!;
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
    public CustomDesignRequestType Type { get; private set; }
    public string? Note { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public CustomDesignRequirement? CustomDesignRequirement { get; private set; }
    public ICollection<CustomDesignAsset> CustomDesignAssets { get; private set; } = new List<CustomDesignAsset>();

    private CustomDesignRequest(
        CustomDesignRequestId id,
        string code,
        Guid customerId,
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
        CustomDesignRequestType type,
        string? note,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        CustomerId = customerId;
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
        Type = type;
        Note = note;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private CustomDesignRequest() : base()
    {
    }

    public static ResultT<CustomDesignRequest> Create(
        Guid id,
        string code,
        Guid customerId,
        Guid customDesignRequirementId,
        decimal desiredLengthMm,
        decimal desiredWidthMm,
        decimal desiredHeightMm,
        string? sketches,
        string? customerPrompt,
        DateTime desiredDeliveryDate,
        int desiredQuantity,
        decimal targetBudget,
        string type,
        DateTime createdAt,
        DateTime updatedAt)
    {
        // Validate dimensions - must be >= 10mm
        if (desiredLengthMm < 10 || desiredWidthMm < 10 || desiredHeightMm < 10)
            return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.InvalidDimensions());

        // Validate delivery date - cannot be today or in the past
        if (desiredDeliveryDate.Date <= DateTime.UtcNow.Date)
            return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.InvalidDeliveryDate());

        // Validate quantity > 0
        if (desiredQuantity <= 0)
            return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.InvalidQuantity());

        // Validate budget > 0
        if (targetBudget <= 0)
            return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.InvalidBudget());

        // Validate and parse type
        if (!Enum.TryParse<CustomDesignRequestType>(type, true, out var typeEnum))
            return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.InvalidType());

        // Validate sketches and customerPrompt based on type
        if (typeEnum == CustomDesignRequestType.Sketch)
        {
            // For Sketch type: sketches are required
            if (string.IsNullOrWhiteSpace(sketches))
                return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.SketchesRequiredForSketchType());
        }
        else if (typeEnum == CustomDesignRequestType.Idea)
        {
            // For Idea type: customerPrompt is required
            if (string.IsNullOrWhiteSpace(customerPrompt))
                return Result.Failure<CustomDesignRequest>(CustomDesignRequestError.CustomerPromptRequiredForIdeaType());
        }

        return Result.Success(new CustomDesignRequest(
            CustomDesignRequestId.From(id),
            code,
            customerId,
            CustomDesignRequirementId.From(customDesignRequirementId),
            desiredLengthMm,
            desiredWidthMm,
            desiredHeightMm,
            sketches,
            customerPrompt,
            desiredDeliveryDate,
            desiredQuantity,
            targetBudget,
            0, // UsedSupportConceptDesignTime is always 0 when created
            CustomDesignRequestStatus.Submitted,
            typeEnum,
            null, // Note is null when created
            createdAt,
            updatedAt));
    }

    public Result Update(
        decimal? desiredLengthMm,
        decimal? desiredWidthMm,
        decimal? desiredHeightMm,
        string? sketches,
        string? customerPrompt,
        DateTime? desiredDeliveryDate,
        int? desiredQuantity,
        decimal? targetBudget,
        CustomDesignRequestStatus? status,
        string? note,
        bool isStaff = false)
    {
        // Validate that customers can only update when status is MissingInformation
        if (!isStaff && Status != CustomDesignRequestStatus.MissingInformation)
            return Result.Failure(CustomDesignRequestError.CustomerCanOnlyUpdateInMissingInformationStatus());

        bool hasChanges = false;

        // Validate dimensions if provided - must be >= 10mm
        if (desiredLengthMm.HasValue)
        {
            if (desiredLengthMm < 10)
                return Result.Failure(CustomDesignRequestError.InvalidDimensions());
            if (desiredLengthMm.Value != DesiredLengthMm)
            {
                DesiredLengthMm = desiredLengthMm.Value;
                hasChanges = true;
            }
        }

        if (desiredWidthMm.HasValue)
        {
            if (desiredWidthMm < 10)
                return Result.Failure(CustomDesignRequestError.InvalidDimensions());
            if (desiredWidthMm.Value != DesiredWidthMm)
            {
                DesiredWidthMm = desiredWidthMm.Value;
                hasChanges = true;
            }
        }

        if (desiredHeightMm.HasValue)
        {
            if (desiredHeightMm < 10)
                return Result.Failure(CustomDesignRequestError.InvalidDimensions());
            if (desiredHeightMm.Value != DesiredHeightMm)
            {
                DesiredHeightMm = desiredHeightMm.Value;
                hasChanges = true;
            }
        }

        // Validate delivery date if provided - cannot be today or in the past
        if (desiredDeliveryDate.HasValue)
        {
            if (desiredDeliveryDate.Value.Date <= DateTime.UtcNow.Date)
                return Result.Failure(CustomDesignRequestError.InvalidDeliveryDate());
            if (desiredDeliveryDate.Value != DesiredDeliveryDate)
            {
                DesiredDeliveryDate = desiredDeliveryDate.Value;
                hasChanges = true;
            }
        }

        // Validate quantity if provided > 0
        if (desiredQuantity.HasValue)
        {
            if (desiredQuantity <= 0)
                return Result.Failure(CustomDesignRequestError.InvalidQuantity());
            if (desiredQuantity.Value != DesiredQuantity)
            {
                DesiredQuantity = desiredQuantity.Value;
                hasChanges = true;
            }
        }

        // Validate budget if provided > 0
        if (targetBudget.HasValue)
        {
            if (targetBudget <= 0)
                return Result.Failure(CustomDesignRequestError.InvalidBudget());
            if (targetBudget.Value != TargetBudget)
            {
                TargetBudget = targetBudget.Value;
                hasChanges = true;
            }
        }

        // Update sketches if provided - validate for Sketch type
        if (sketches != null)
        {
            if (Type == CustomDesignRequestType.Sketch && string.IsNullOrWhiteSpace(sketches))
                return Result.Failure(CustomDesignRequestError.SketchesRequiredForSketchType());
            Sketches = sketches;
            hasChanges = true;
        }

        // Update customer prompt if provided - validate for Idea type
        if (customerPrompt != null)
        {
            if (Type == CustomDesignRequestType.Idea && string.IsNullOrWhiteSpace(customerPrompt))
                return Result.Failure(CustomDesignRequestError.CustomerPromptRequiredForIdeaType());
            CustomerPrompt = customerPrompt;
            hasChanges = true;
        }

        // Update status if provided - only staff can update status
        if (status.HasValue && status.Value != Status)
        {
            if (!isStaff)
                return Result.Failure(CustomDesignRequestError.UnauthorizedStatusUpdate());
            
            // When changing to Rejected or MissingInformation, note is required
            if ((status.Value == CustomDesignRequestStatus.Rejected || status.Value == CustomDesignRequestStatus.MissingInformation)
                && string.IsNullOrWhiteSpace(note))
            {
                return Result.Failure(CustomDesignRequestError.NoteRequiredForStatusChange());
            }
            
            Status = status.Value;
            
            // Raise event when status changes to Approved
            if (status.Value == CustomDesignRequestStatus.Approved)
            {
                RaiseDomainEvent(new CustomDesignRequestApprovedDomainEvent(
                    Id.Value,
                    CustomerId,
                    Type,
                    Sketches,
                    CustomerPrompt));
            }
            
            hasChanges = true;
        }

        // Update note if provided
        if (note != null)
        {
            Note = note;
            hasChanges = true;
        }

        if (!hasChanges)
            return Result.Failure(CustomDesignRequestError.NothingToUpdate());

        // If customer updates, reset status to Submitted
        if (!isStaff)
        {
            Status = CustomDesignRequestStatus.Submitted;
        }

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void IncrementUsedSupportConceptDesignTime()
    {
        UsedSupportConceptDesignTime++;
    }
}
