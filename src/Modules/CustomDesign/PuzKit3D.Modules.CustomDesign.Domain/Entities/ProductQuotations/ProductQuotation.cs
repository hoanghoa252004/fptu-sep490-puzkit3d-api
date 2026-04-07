using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;

public sealed class ProductQuotation : AggregateRoot<ProductQuotationId>
{
    public ProposalId ProposalId { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public decimal Volume { get; private set; }
    public Guid MaterialId { get; private set; }
    public decimal MaterialBasePrice { get; private set; }
    public decimal BaseAmount { get; private set; }
    public decimal WeightPercent { get; private set; }
    public decimal WeightAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Proposal? Proposal { get; private set; }

    private ProductQuotation(
        ProductQuotationId id,
        ProposalId proposalId,
        string code,
        decimal volume,
        Guid materialId,
        decimal materialBasePrice,
        decimal baseAmount,
        decimal weightPercent,
        decimal weightAmount,
        decimal totalAmount,
        DateTime createdAt) : base(id)
    {
        ProposalId = proposalId;
        Code = code;
        Volume = volume;
        MaterialId = materialId;
        MaterialBasePrice = materialBasePrice;
        BaseAmount = baseAmount;
        WeightPercent = weightPercent;
        WeightAmount = weightAmount;
        TotalAmount = totalAmount;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private ProductQuotation() : base()
    {
    }

    public static ResultT<ProductQuotation> Create(
        ProposalId proposalId,
        string code,
        decimal volume,
        Guid materialId,
        decimal materialBasePrice,
        decimal baseAmount,
        decimal weightPercent,
        decimal weightAmount,
        decimal totalAmount,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidCode());

        if (volume <= 0)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidVolume());

        if (materialId == Guid.Empty)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidMaterialId());

        if (materialBasePrice < 0)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidMaterialBasePrice());

        if (baseAmount < 0)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidBaseAmount());

        if (weightPercent < 0 || weightPercent > 100)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidWeightPercent());

        if (weightAmount < 0)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidWeightAmount());

        if (totalAmount < 0)
            return Result.Failure<ProductQuotation>(ProductQuotationError.InvalidTotalAmount());

        var productQuotationId = ProductQuotationId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var productQuotation = new ProductQuotation(
            productQuotationId,
            proposalId,
            code,
            volume,
            materialId,
            materialBasePrice,
            baseAmount,
            weightPercent,
            weightAmount,
            totalAmount,
            timestamp);

        return Result.Success(productQuotation);
    }

    public Result Update(
        decimal? volume = null,
        decimal? materialBasePrice = null,
        decimal? baseAmount = null,
        decimal? weightPercent = null,
        decimal? weightAmount = null,
        decimal? totalAmount = null)
    {
        if (volume.HasValue)
        {
            if (volume.Value <= 0)
                return Result.Failure(ProductQuotationError.InvalidVolume());
            Volume = volume.Value;
        }

        if (materialBasePrice.HasValue)
        {
            if (materialBasePrice.Value < 0)
                return Result.Failure(ProductQuotationError.InvalidMaterialBasePrice());
            MaterialBasePrice = materialBasePrice.Value;
        }

        if (baseAmount.HasValue)
        {
            if (baseAmount.Value < 0)
                return Result.Failure(ProductQuotationError.InvalidBaseAmount());
            BaseAmount = baseAmount.Value;
        }

        if (weightPercent.HasValue)
        {
            if (weightPercent.Value < 0 || weightPercent.Value > 100)
                return Result.Failure(ProductQuotationError.InvalidWeightPercent());
            WeightPercent = weightPercent.Value;
        }

        if (weightAmount.HasValue)
        {
            if (weightAmount.Value < 0)
                return Result.Failure(ProductQuotationError.InvalidWeightAmount());
            WeightAmount = weightAmount.Value;
        }

        if (totalAmount.HasValue)
        {
            if (totalAmount.Value < 0)
                return Result.Failure(ProductQuotationError.InvalidTotalAmount());
            TotalAmount = totalAmount.Value;
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
