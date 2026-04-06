using MediatR;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

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

    public static ResultT<CustomDesignRequirement> Create(
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
        if (minPartQuantity <= 0)
            return Result.Failure<CustomDesignRequirement>(CustomDesignRequirementError.InvalidMinPartQuantity());

        if (maxPartQuantity <= 0)
            return Result.Failure<CustomDesignRequirement>(CustomDesignRequirementError.InvalidMaxPartQuantity());

        if(maxPartQuantity <= minPartQuantity)
            return Result.Failure<CustomDesignRequirement>(CustomDesignRequirementError.InvalidQuantityRange());

        return Result.Success(new CustomDesignRequirement(
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
            updatedAt));
    }

    public Result Update(
        Guid? topicId,
        Guid? materialId,
        Guid? assemblyMethodId,
        string? difficulty,
        int? minPartQuantity,
        int? maxPartQuantity,
        bool? isActive,
        DateTime updatedAt)
    {
        bool hasChanges = false;

        // Validate difficulty if provided
        DifficultyLevel? difficultyParsed = null;
        if (difficulty is not null)
        {
            if(!Enum.TryParse<DifficultyLevel>(difficulty, true, out var parsed))
                return Result.Failure(CustomDesignRequirementError.InvalidDifficulty(difficulty.ToString()));
            difficultyParsed = parsed;
        }

        // Validate minPartQuantity if provided
        if (minPartQuantity.HasValue)
        {
            if (minPartQuantity <= 0)
                return Result.Failure(CustomDesignRequirementError.InvalidMinPartQuantity());

            if (maxPartQuantity.HasValue == false && minPartQuantity >= MaxPartQuantity)
                return Result.Failure(CustomDesignRequirementError.InvalidQuantityRange());
        }


        // Validate maxPartQuantity if provided
        if (maxPartQuantity.HasValue)
        {
            if (maxPartQuantity <= 0)
                return Result.Failure(CustomDesignRequirementError.InvalidMaxPartQuantity());

            if (minPartQuantity.HasValue == false && MinPartQuantity >= maxPartQuantity)
                return Result.Failure(CustomDesignRequirementError.InvalidQuantityRange());
        }

        // Validate quantity range if both provided
        if (minPartQuantity.HasValue && maxPartQuantity.HasValue)
        {
            if (minPartQuantity > maxPartQuantity)
                return Result.Failure(CustomDesignRequirementError.InvalidQuantityRange());
        }

        // Or validate range if only one is provided but comparing with current value
        else if (minPartQuantity.HasValue && minPartQuantity > (maxPartQuantity ?? MaxPartQuantity))
            return Result.Failure(CustomDesignRequirementError.InvalidQuantityRange());
        else if (maxPartQuantity.HasValue && maxPartQuantity < (minPartQuantity ?? MinPartQuantity))
            return Result.Failure(CustomDesignRequirementError.InvalidQuantityRange());

        // Update only provided fields and track changes
        if (topicId.HasValue && topicId.Value != TopicId)
        {
            TopicId = topicId.Value;
            hasChanges = true;
        }

        if (materialId.HasValue && materialId.Value != MaterialId)
        {
            MaterialId = materialId.Value;
            hasChanges = true;
        }

        if (assemblyMethodId.HasValue && assemblyMethodId.Value != AssemblyMethodId)
        {
            AssemblyMethodId = assemblyMethodId.Value;
            hasChanges = true;
        }

        if (difficulty is not null && difficultyParsed != Difficulty)
        {
            Difficulty = difficultyParsed!.Value;
            hasChanges = true;
        }

        if (minPartQuantity.HasValue && minPartQuantity.Value != MinPartQuantity)
        {
            MinPartQuantity = minPartQuantity.Value;
            hasChanges = true;
        }

        if (maxPartQuantity.HasValue && maxPartQuantity.Value != MaxPartQuantity)
        {
            MaxPartQuantity = maxPartQuantity.Value;
            hasChanges = true;
        }

        if (isActive.HasValue && isActive.Value != IsActive)
        {
            IsActive = isActive.Value;
            hasChanges = true;
        }

        if (!hasChanges)
            return Result.Failure(CustomDesignRequirementError.NothingToUpdate());

        UpdatedAt = updatedAt;

        return Result.Success();
    }
}
