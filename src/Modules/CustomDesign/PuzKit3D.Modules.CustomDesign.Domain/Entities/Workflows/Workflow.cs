using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

public sealed class Workflow : AggregateRoot<WorkflowId>
{
    public ProposalId ProposalId { get; private set; } = null!;
    public PhaseId PhaseId { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public string? Outcome { get; private set; }
    public WorkflowStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Workflow(
        WorkflowId id,
        ProposalId proposalId,
        PhaseId phaseId,
        DateTime startDate,
        DateTime? endDate,
        string? description,
        string? outcome,
        WorkflowStatus status,
        DateTime createdAt) : base(id)
    {
        ProposalId = proposalId;
        PhaseId = phaseId;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        Outcome = outcome;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Workflow() : base()
    {
    }

    public static ResultT<Workflow> Create(
        ProposalId proposalId,
        PhaseId phaseId,
        DateTime startDate,
        string? description = null,
        DateTime? createdAt = null)
    {
        if (startDate == default)
            return Result.Failure<Workflow>(WorkflowError.InvalidStartDate());

        var workflowId = WorkflowId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var workflow = new Workflow(
            workflowId,
            proposalId,
            phaseId,
            startDate,
            null,
            description,
            null,
            WorkflowStatus.Draft,
            timestamp);

        return Result.Success(workflow);
    }

    public Result UpdateStatus(WorkflowStatus newStatus)
    {
        if (Status == newStatus)
            return Result.Failure(WorkflowError.InvalidStatusTransition());

        if (!WorkflowStatusTransition.IsValidTransition(Status, newStatus))
            return Result.Failure(WorkflowError.InvalidStatusTransition());

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Set EndDate when workflow is completed or cancelled
        if (newStatus == WorkflowStatus.Completed ||
            newStatus == WorkflowStatus.CancelledByCustomer ||
            newStatus == WorkflowStatus.CancelledByStaff ||
            newStatus == WorkflowStatus.RejectedByCustomer)
        {
            EndDate = DateTime.UtcNow;
        }

        return Result.Success();
    }

    public Result Start()
    {
        return UpdateStatus(WorkflowStatus.InProgress);
    }

    public Result Complete(string? outcome = null)
    {
        if (outcome != null)
        {
            Outcome = outcome;
        }
        return UpdateStatus(WorkflowStatus.Completed);
    }

    public Result Ready()
    {
        return UpdateStatus(WorkflowStatus.ReadyToStart);
    }

    public Result MarkAsDone(string? outcome = null)
    {
        if (outcome != null)
        {
            Outcome = outcome;
        }
        return UpdateStatus(WorkflowStatus.Done);
    }

    public Result RejectByCustomer()
    {
        return UpdateStatus(WorkflowStatus.RejectedByCustomer);
    }

    public Result CancelByStaff()
    {
        return UpdateStatus(WorkflowStatus.CancelledByStaff);
    }

    public Result CancelByCustomer()
    {
        return UpdateStatus(WorkflowStatus.CancelledByCustomer);
    }

    public Result Update(
        string? description = null,
        string? outcome = null)
    {
        if (description != null)
        {
            Description = description;
        }

        if (outcome != null)
        {
            Outcome = outcome;
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
