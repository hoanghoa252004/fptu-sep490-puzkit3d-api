using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public class PartnerProductOrderDetail : Entity<PartnerProductOrderDetailId>
{
    public PartnerProductOrderId PartnerProductOrderId { get; private set; } = null!;
    public PartnerProductId PartnerProductId { get; private set; } = null!;
    public string? PartnerProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalAmount { get; private set; }

    private PartnerProductOrderDetail(
        PartnerProductOrderDetailId id,
        PartnerProductOrderId partnerProductOrderId,
        PartnerProductId partnerProductId,
        string? partnerProductName,
        decimal unitPrice,
        int quantity,
        decimal totalAmount) : base(id)
    {
        PartnerProductOrderId = partnerProductOrderId;
        PartnerProductId = partnerProductId;
        PartnerProductName = partnerProductName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        TotalAmount = totalAmount;
    }

    private PartnerProductOrderDetail() : base()
    {
    }

    public static ResultT<PartnerProductOrderDetail> Create(
        PartnerProductOrderId partnerProductOrderId,
        PartnerProductId partnerProductId,
        string? partnerProductName,
        decimal unitPrice,
        int quantity)
    {
        if (quantity <= 0)
            return Result.Failure<PartnerProductOrderDetail>(PartnerProductOrderDetailError.InvalidQuantity());

        if (unitPrice < 0)
            return Result.Failure<PartnerProductOrderDetail>(PartnerProductOrderDetailError.InvalidPrice());

        var detailId = PartnerProductOrderDetailId.Create();
        var totalAmount = unitPrice * quantity;

        var detail = new PartnerProductOrderDetail(
            detailId,
            partnerProductOrderId,
            partnerProductId,
            partnerProductName,
            unitPrice,
            quantity,
            totalAmount);

        return Result.Success(detail);
    }
}
