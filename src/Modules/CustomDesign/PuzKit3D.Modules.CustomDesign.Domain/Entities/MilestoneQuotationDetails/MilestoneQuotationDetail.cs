using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;

public sealed class MilestoneQuotationDetail : AggregateRoot<MilestoneQuotationDetailId>
{
    public MilestoneQuotationId MilestoneQuotationId { get; private set; }
    public MilestoneId MilestoneId { get; private set; }
    public decimal LaborCost { get; private set; }
    public decimal WeightPercent { get; private set; }
    public decimal WeightAmount { get; private set; }
    public decimal TotalAmount { get; private set; }

    private MilestoneQuotationDetail(
        MilestoneQuotationDetailId id,
        MilestoneQuotationId milestoneQuotationId,
        MilestoneId milestoneId,
        decimal laborCost,
        decimal weightPercent,
        decimal weightAmount,
        decimal totalAmount) : base(id)
    {
        MilestoneQuotationId = milestoneQuotationId;
        MilestoneId = milestoneId;
        LaborCost = laborCost;
        WeightPercent = weightPercent;
        WeightAmount = weightAmount;
        TotalAmount = totalAmount;
    }

    private MilestoneQuotationDetail() : base()
    {
    }

    public static ResultT<MilestoneQuotationDetail> Create(
        MilestoneQuotationId milestoneQuotationId,
        MilestoneId milestoneId,
        decimal laborCost,
        decimal weightPercent,
        decimal weightAmount,
        decimal totalAmount)
    {
        if (laborCost < 0)
            return Result.Failure<MilestoneQuotationDetail>(MilestoneQuotationDetailError.InvalidLaborCost());

        if (weightPercent < 0 || weightPercent > 100)
            return Result.Failure<MilestoneQuotationDetail>(MilestoneQuotationDetailError.InvalidWeightPercent());

        if (weightAmount < 0)
            return Result.Failure<MilestoneQuotationDetail>(MilestoneQuotationDetailError.InvalidWeightAmount());

        if (totalAmount < 0)
            return Result.Failure<MilestoneQuotationDetail>(MilestoneQuotationDetailError.InvalidTotalAmount());

        var detailId = MilestoneQuotationDetailId.Create();
        var detail = new MilestoneQuotationDetail(
            detailId,
            milestoneQuotationId,
            milestoneId,
            laborCost,
            weightPercent,
            weightAmount,
            totalAmount);

        return Result.Success(detail);
    }

    public Result Update(decimal? laborCost = null, decimal? weightPercent = null, decimal? weightAmount = null, decimal? totalAmount = null)
    {
        if (laborCost.HasValue)
        {
            if (laborCost.Value < 0)
                return Result.Failure(MilestoneQuotationDetailError.InvalidLaborCost());
            LaborCost = laborCost.Value;
        }

        if (weightPercent.HasValue)
        {
            if (weightPercent.Value < 0 || weightPercent.Value > 100)
                return Result.Failure(MilestoneQuotationDetailError.InvalidWeightPercent());
            WeightPercent = weightPercent.Value;
        }

        if (weightAmount.HasValue)
        {
            if (weightAmount.Value < 0)
                return Result.Failure(MilestoneQuotationDetailError.InvalidWeightAmount());
            WeightAmount = weightAmount.Value;
        }

        if (totalAmount.HasValue)
        {
            if (totalAmount.Value < 0)
                return Result.Failure(MilestoneQuotationDetailError.InvalidTotalAmount());
            TotalAmount = totalAmount.Value;
        }

        return Result.Success();
    }
}
