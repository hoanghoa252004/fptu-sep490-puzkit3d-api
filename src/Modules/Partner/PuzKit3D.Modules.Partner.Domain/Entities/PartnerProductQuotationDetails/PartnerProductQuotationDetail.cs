using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;

public class PartnerProductQuotationDetail : Entity<PartnerProductQuotationDetailId>
{
    public PartnerProductQuotationId PartnerProductQuotationId { get; private set; } = null!;
    public PartnerProductId PartnerProductId { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalAmount { get; private set; }

    private PartnerProductQuotationDetail(
        PartnerProductQuotationDetailId id,
        PartnerProductQuotationId partnerProductQuotationId,
        PartnerProductId partnerProductId,
        int quantity,
        decimal unitPrice,
        decimal totalAmount) : base(id)
    {
        PartnerProductQuotationId = partnerProductQuotationId;
        PartnerProductId = partnerProductId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalAmount = totalAmount;
    }

    private PartnerProductQuotationDetail() : base()
    {
    }

    public static ResultT<PartnerProductQuotationDetail> Create(
        PartnerProductQuotationId partnerProductQuotationId,
        PartnerProductId partnerProductId,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            return Result.Failure<PartnerProductQuotationDetail>(PartnerProductQuotationDetailError.InvalidQuantity());

        if (unitPrice < 0)
            return Result.Failure<PartnerProductQuotationDetail>(PartnerProductQuotationDetailError.InvalidPrice());

        var detailId = PartnerProductQuotationDetailId.Create();
        var totalAmount = unitPrice * quantity;

        var detail = new PartnerProductQuotationDetail(
            detailId,
            partnerProductQuotationId,
            partnerProductId,
            quantity,
            unitPrice,
            totalAmount);

        return Result.Success(detail);
    }

    public Result UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(PartnerProductQuotationDetailError.InvalidQuantity());

        Quantity = quantity;
        TotalAmount = UnitPrice * quantity;

        return Result.Success();
    }
}
