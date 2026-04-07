using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

public sealed class Milestone : AggregateRoot<MilestoneId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int SequenceOrder { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation property (private backing field with read-only collection)
    private readonly List<Phase> _phases = new();
    public IReadOnlyList<Phase> Phases => _phases;

    private Milestone(
        MilestoneId id,
        string name,
        string? description,
        int sequenceOrder,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Description = description;
        SequenceOrder = sequenceOrder;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Milestone() : base()
    {
    }

    public static ResultT<Milestone> Create(
        string name,
        int sequenceOrder,
        string? description = null,
        bool isActive = true,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Milestone>(MilestoneError.InvalidName());

        if (sequenceOrder <= 0)
            return Result.Failure<Milestone>(MilestoneError.InvalidSequenceOrder());

        var milestoneId = MilestoneId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var milestone = new Milestone(
            milestoneId,
            name,
            description,
            sequenceOrder,
            isActive,
            timestamp);

        return Result.Success(milestone);
    }

    public Result Update(
        string name,
        int sequenceOrder,
        bool isActive = false,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(MilestoneError.InvalidName());
        if (sequenceOrder <= 0)
            return Result.Failure(MilestoneError.InvalidSequenceOrder());
        Name = name;
        SequenceOrder = sequenceOrder;
        IsActive = isActive;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void AddPhase(Phase phase)
    {
        _phases.Add(phase);
    }
}
