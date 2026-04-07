using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

public sealed class Phase : AggregateRoot<PhaseId>
{
    public MilestoneId MilestoneId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int SequenceOrder { get; private set; }
    public decimal BasePrice { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Phase(
        PhaseId id,
        MilestoneId milestoneId,
        string name,
        string? description,
        int sequenceOrder,
        decimal basePrice,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        MilestoneId = milestoneId;
        Name = name;
        Description = description;
        SequenceOrder = sequenceOrder;
        BasePrice = basePrice;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Phase() : base()
    {
    }

    public static ResultT<Phase> Create(
        MilestoneId milestoneId,
        string name,
        int sequenceOrder,
        decimal basePrice,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Phase>(PhaseError.InvalidName());

        if (sequenceOrder <= 0)
            return Result.Failure<Phase>(PhaseError.InvalidSequenceOrder());

        if (basePrice < 0)
            return Result.Failure<Phase>(PhaseError.InvalidBasePrice());

        var phaseId = PhaseId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var phase = new Phase(
            phaseId,
            milestoneId,
            name,
            description,
            sequenceOrder,
            basePrice,
            isActive,
            timestamp);

        return Result.Success(phase);
    }

    public Result Update(
        string name,
        int sequenceOrder,
        decimal basePrice,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PhaseError.InvalidName());

        if (sequenceOrder <= 0)
            return Result.Failure(PhaseError.InvalidSequenceOrder());

        if (basePrice < 0)
            return Result.Failure(PhaseError.InvalidBasePrice());
        Name = name;
        SequenceOrder = sequenceOrder;
        BasePrice = basePrice;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
