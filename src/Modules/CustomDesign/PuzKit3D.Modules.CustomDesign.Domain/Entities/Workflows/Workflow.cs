using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
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
            WorkflowStatus.NotStarted,
            timestamp);

        return Result.Success(workflow);
    }

    public Result Start()
    {
        if (Status != WorkflowStatus.NotStarted)
            return Result.Failure(WorkflowError.CannotStartWorkflow());

        Status = WorkflowStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Complete(string? outcome = null)
    {
        if (Status != WorkflowStatus.InProgress && Status != WorkflowStatus.OnHold)
            return Result.Failure(WorkflowError.CannotCompleteWorkflow());

        Status = WorkflowStatus.Completed;
        EndDate = DateTime.UtcNow;
        Outcome = outcome;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result PutOnHold()
    {
        if (Status != WorkflowStatus.InProgress)
            return Result.Failure(WorkflowError.CannotPutOnHold());

        Status = WorkflowStatus.OnHold;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Resume()
    {
        if (Status != WorkflowStatus.OnHold)
            return Result.Failure(WorkflowError.CannotResumeWorkflow());

        Status = WorkflowStatus.InProgress;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Fail(string? outcome = null)
    {
        if (Status == WorkflowStatus.Completed || Status == WorkflowStatus.Cancelled || Status == WorkflowStatus.Failed)
            return Result.Failure(WorkflowError.CannotFailWorkflow());

        Status = WorkflowStatus.Failed;
        EndDate = DateTime.UtcNow;
        Outcome = outcome;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status == WorkflowStatus.Completed || Status == WorkflowStatus.Cancelled)
            return Result.Failure(WorkflowError.CannotCancelWorkflow());

        Status = WorkflowStatus.Cancelled;
        EndDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
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
