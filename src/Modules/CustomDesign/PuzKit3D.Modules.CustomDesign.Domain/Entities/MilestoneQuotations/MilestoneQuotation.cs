using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;

public sealed class MilestoneQuotation : AggregateRoot<MilestoneQuotationId>
{
    public ProposalId ProposalId { get; private set; }
    public string Code { get; private set; } = null!;
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation property (private backing field with read-only collection)
    private readonly List<MilestoneQuotationDetails.MilestoneQuotationDetail> _details = new();
    public IReadOnlyList<MilestoneQuotationDetails.MilestoneQuotationDetail> Details => _details;

    private MilestoneQuotation(
        MilestoneQuotationId id,
        ProposalId proposalId,
        string code,
        decimal totalAmount,
        DateTime createdAt) : base(id)
    {
        ProposalId = proposalId;
        Code = code;
        TotalAmount = totalAmount;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private MilestoneQuotation() : base()
    {
    }

    public static ResultT<MilestoneQuotation> Create(
        ProposalId proposalId,
        string code,
        decimal totalAmount,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<MilestoneQuotation>(MilestoneQuotationError.InvalidCode());

        if (totalAmount < 0)
            return Result.Failure<MilestoneQuotation>(MilestoneQuotationError.InvalidTotalAmount());

        var milestoneQuotationId = MilestoneQuotationId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var milestoneQuotation = new MilestoneQuotation(
            milestoneQuotationId,
            proposalId,
            code,
            totalAmount,
            timestamp);

        return Result.Success(milestoneQuotation);
    }

    public Result UpdateTotalAmount(decimal newTotalAmount)
    {
        if (newTotalAmount < 0)
            return Result.Failure(MilestoneQuotationError.InvalidTotalAmount());

        TotalAmount = newTotalAmount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void AddDetail(MilestoneQuotationDetails.MilestoneQuotationDetail detail)
    {
        _details.Add(detail);
    }
}
