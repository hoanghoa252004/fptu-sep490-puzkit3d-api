using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;

public class PartnerProductRequestDetail : Entity<PartnerProductRequestDetailId>
{
    public PartnerProductRequestId PartnerProductRequestId { get; private set; } = null!;
    public PartnerProductId PartnerProductId { get; private set; } = null!;
    public decimal ReferenceUnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal ReferenceTotalAmount { get; private set; }

    private PartnerProductRequestDetail(
        PartnerProductRequestDetailId id,
        PartnerProductRequestId partnerProductRequestId,
        PartnerProductId partnerProductId,
        decimal referenceUnitPrice,
        int quantity,
        decimal referenceTotalAmount) : base(id)
    {
        PartnerProductRequestId = partnerProductRequestId;
        PartnerProductId = partnerProductId;
        ReferenceUnitPrice = referenceUnitPrice;
        Quantity = quantity;
        ReferenceTotalAmount = referenceTotalAmount;
    }

    private PartnerProductRequestDetail() : base()
    {
    }

    public static ResultT<PartnerProductRequestDetail> Create(
        PartnerProductRequestId partnerProductRequestId,
        PartnerProductId partnerProductId,
        decimal referenceUnitPrice,
        int quantity)
    {
        if (referenceUnitPrice < 0)
            return Result.Failure<PartnerProductRequestDetail>(PartnerProductRequestDetailError.InvalidPrice());

        if (quantity <= 0)
            return Result.Failure<PartnerProductRequestDetail>(PartnerProductRequestDetailError.InvalidQuantity());

        var detailId = PartnerProductRequestDetailId.Create();
        var referenceTotalAmount = referenceUnitPrice * quantity;

        var detail = new PartnerProductRequestDetail(
            detailId,
            partnerProductRequestId,
            partnerProductId,
            referenceUnitPrice,
            quantity,
            referenceTotalAmount);

        return Result.Success(detail);
    }

    public Result UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(PartnerProductRequestDetailError.InvalidQuantity());

        Quantity = quantity;
        ReferenceTotalAmount = ReferenceUnitPrice * quantity;

        return Result.Success();
    }
}
