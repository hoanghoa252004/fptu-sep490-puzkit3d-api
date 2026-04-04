using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

public sealed class CustomDesignRequirement : Entity<CustomDesignRequirementId>
{
    public string Code { get; private set; } = null!;
    public Guid TopicId { get; private set; }
    public Guid MaterialId { get; private set; }
    public Guid AssemblyMethodId { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public int MinPartQuantity { get; private set; }
    public int MaxPartQuantity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private CustomDesignRequirement(
        CustomDesignRequirementId id,
        string code,
        Guid topicId,
        Guid materialId,
        Guid assemblyMethodId,
        DifficultyLevel difficulty,
        int minPartQuantity,
        int maxPartQuantity,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Code = code;
        TopicId = topicId;
        MaterialId = materialId;
        AssemblyMethodId = assemblyMethodId;
        Difficulty = difficulty;
        MinPartQuantity = minPartQuantity;
        MaxPartQuantity = maxPartQuantity;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private CustomDesignRequirement() : base()
    {
    }

    public static CustomDesignRequirement Create(
        Guid id,
        string code,
        Guid topicId,
        Guid materialId,
        Guid assemblyMethodId,
        DifficultyLevel difficulty,
        int minPartQuantity,
        int maxPartQuantity,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new CustomDesignRequirement(
            CustomDesignRequirementId.From(id),
            code,
            topicId,
            materialId,
            assemblyMethodId,
            difficulty,
            minPartQuantity,
            maxPartQuantity,
            isActive,
            createdAt,
            updatedAt);
    }

    public void Update(
        Guid topicId,
        Guid materialId,
        Guid assemblyMethodId,
        DifficultyLevel difficulty,
        int minPartQuantity,
        int maxPartQuantity,
        bool isActive,
        DateTime updatedAt)
    {
        TopicId = topicId;
        MaterialId = materialId;
        AssemblyMethodId = assemblyMethodId;
        Difficulty = difficulty;
        MinPartQuantity = minPartQuantity;
        MaxPartQuantity = maxPartQuantity;
        IsActive = isActive;
        UpdatedAt = updatedAt;
    }
}
