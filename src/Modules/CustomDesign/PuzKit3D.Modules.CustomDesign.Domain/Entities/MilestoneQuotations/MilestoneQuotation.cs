using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;

public sealed class MilestoneQuotation : AggregateRoot<MilestoneQuotationId>
{
    public ProposalId ProposalId { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<MilestoneQuotationDetail> _details = new();
    public IReadOnlyList<MilestoneQuotationDetail> Details => _details;

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
