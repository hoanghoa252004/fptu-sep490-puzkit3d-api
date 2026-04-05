namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetAllRequirements;

public sealed record GetAllCustomDesignRequirementsResponseDto(
    Guid Id,
    string Code,
    Guid TopicId,
    Guid MaterialId,
    Guid AssemblyMethodId,
    string Difficulty,
    int MinPartQuantity,
    int MaxPartQuantity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IEnumerable<Guid> CapabilityIds);
