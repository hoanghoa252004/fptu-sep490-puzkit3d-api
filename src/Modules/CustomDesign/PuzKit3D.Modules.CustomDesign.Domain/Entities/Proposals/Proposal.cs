using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

public sealed class Proposal : AggregateRoot<ProposalId>
{
    public CustomDesignRequestId RequestId { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public decimal LaborCost { get; private set; }
    public decimal ProductCost { get; private set; }
    public decimal TotalWeightPercent { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime? ManagerApprovedAt { get; private set; }
    public DateTime? CustomerApprovedAt { get; private set; }
    public string? Note { get; private set; }
    public ProposalStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<MilestoneQuotation> _milestoneQuotations = new();
    public IReadOnlyList<MilestoneQuotation> MilestoneQuotations => _milestoneQuotations;

    private readonly List<ProductQuotation> _productQuotations = new();
    public IReadOnlyList<ProductQuotation> ProductQuotations => _productQuotations;

    private readonly List<Workflow> _workflows = new();
    public IReadOnlyList<Workflow> Workflows => _workflows;

    private Proposal(
        ProposalId id,
        CustomDesignRequestId requestId,
        string code,
        decimal laborCost,
        decimal productCost,
        decimal totalWeightPercent,
        decimal totalAmount,
        ProposalStatus status,
        string? note,
        DateTime createdAt) : base(id)
    {
        RequestId = requestId;
        Code = code;
        LaborCost = laborCost;
        ProductCost = productCost;
        TotalWeightPercent = totalWeightPercent;
        TotalAmount = totalAmount;
        Status = status;
        Note = note;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Proposal() : base()
    {
    }

    public static ResultT<Proposal> Create(
        CustomDesignRequestId requestId,
        string code,
        decimal laborCost,
        decimal productCost,
        decimal totalWeightPercent,
        decimal totalAmount,
        string? note = null,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<Proposal>(ProposalError.InvalidCode());

        if (laborCost < 0)
            return Result.Failure<Proposal>(ProposalError.InvalidLaborCost());

        if (productCost < 0)
            return Result.Failure<Proposal>(ProposalError.InvalidProductCost());

        if (totalWeightPercent < 0 || totalWeightPercent > 100)
            return Result.Failure<Proposal>(ProposalError.InvalidTotalWeightPercent());

        if (totalAmount < 0)
            return Result.Failure<Proposal>(ProposalError.InvalidTotalAmount());

        var proposalId = ProposalId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var proposal = new Proposal(
            proposalId,
            requestId,
            code,
            laborCost,
            productCost,
            totalWeightPercent,
            totalAmount,
            ProposalStatus.Draft,
            note,
            timestamp);

        return Result.Success(proposal);
    }

    public Result UpdateStatus(ProposalStatus newStatus)
    {
        if (Status == newStatus)
            return Result.Failure(ProposalError.InvalidStatusTransition());

        if (!ProposalStatusTransition.IsValidTransition(Status, newStatus))
            return Result.Failure(ProposalError.InvalidStatusTransition());

        var oldStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Handle approval timestamps
        if (newStatus == ProposalStatus.ApprovedByManager)
        {
            ManagerApprovedAt = DateTime.UtcNow;
        }

        if (newStatus == ProposalStatus.ApprovedByCustomer)
        {
            CustomerApprovedAt = DateTime.UtcNow;
        }

        return Result.Success();
    }

    public Result ApproveByManager()
    {
        return UpdateStatus(ProposalStatus.ApprovedByManager);
    }

    public Result ApproveByCustomer()
    {
        return UpdateStatus(ProposalStatus.ApprovedByCustomer);
    }

    public Result RejectByManager()
    {
        return UpdateStatus(ProposalStatus.RejectedByManager);
    }

    public Result RejectByCustomer()
    {
        return UpdateStatus(ProposalStatus.RejectedByCustomer);
    }

    public Result Cancel()
    {
        return UpdateStatus(ProposalStatus.Cancelled);
    }

    public Result Expire()
    {
        return UpdateStatus(ProposalStatus.Expired);
    }

    public Result Update(decimal? laborCost = null, decimal? productCost = null, decimal? totalWeightPercent = null, decimal? totalAmount = null, string? note = null)
    {
        if (Status != ProposalStatus.Draft)
            return Result.Failure(ProposalError.CannotUpdateProposal());

        if (laborCost.HasValue)
        {
            if (laborCost.Value < 0)
                return Result.Failure(ProposalError.InvalidLaborCost());
            LaborCost = laborCost.Value;
        }

        if (productCost.HasValue)
        {
            if (productCost.Value < 0)
                return Result.Failure(ProposalError.InvalidProductCost());
            ProductCost = productCost.Value;
        }

        if (totalWeightPercent.HasValue)
        {
            if (totalWeightPercent.Value < 0 || totalWeightPercent.Value > 100)
                return Result.Failure(ProposalError.InvalidTotalWeightPercent());
            TotalWeightPercent = totalWeightPercent.Value;
        }

        if (totalAmount.HasValue)
        {
            if (totalAmount.Value < 0)
                return Result.Failure(ProposalError.InvalidTotalAmount());
            TotalAmount = totalAmount.Value;
        }

        if (note != null)
        {
            Note = note;
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void AddMilestoneQuotation(MilestoneQuotation milestoneQuotation)
    {
        _milestoneQuotations.Add(milestoneQuotation);
    }

    public void AddProductQuotation(ProductQuotation productQuotation)
    {
        _productQuotations.Add(productQuotation);
    }

    public void AddWorkflow(Workflow workflow)
    {
        _workflows.Add(workflow);
    }
}
