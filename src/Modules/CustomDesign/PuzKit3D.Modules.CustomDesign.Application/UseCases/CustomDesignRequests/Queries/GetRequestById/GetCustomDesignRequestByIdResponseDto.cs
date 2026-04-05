namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetRequestById;

public sealed record GetCustomDesignRequestByIdResponseDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    Guid CustomDesignRequirementId,
    decimal DesiredLengthMm,
    decimal DesiredWidthMm,
    decimal DesiredHeightMm,
    List<string>? SketchesUrls,
    string? CustomerPrompt,
    DateTime DesiredDeliveryDate,
    int DesiredQuantity,
    decimal TargetBudget,
    int UsedSupportConceptDesignTime,
    string Status,
    string Type,
    DateTime CreatedAt,
    DateTime UpdatedAt);


