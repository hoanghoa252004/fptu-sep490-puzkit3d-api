using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public class PartnerProductQuotation : AggregateRoot<PartnerProductQuotationId>
{
    public string Code { get; private set; } = null!;
    public PartnerProductRequestId PartnerProductRequestId { get; private set; } = null!;
    public int Version { get; private set; }
    public decimal SubTotalAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal ImportTaxAmount { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public DateTime ExpectedDeliveryDate { get; private set; }
    public string? Note { get; private set; }
    public int Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PartnerProductQuotation(
        PartnerProductQuotationId id,
        string code,
        PartnerProductRequestId partnerProductRequestId,
        int version,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal importTaxAmount,
        decimal grandTotalAmount,
        DateTime expectedDeliveryDate,
        string? note,
        int status,
        DateTime createdAt) : base(id)
    {
        Code = code;
        PartnerProductRequestId = partnerProductRequestId;
        Version = version;
        SubTotalAmount = subTotalAmount;
        ShippingFee = shippingFee;
        ImportTaxAmount = importTaxAmount;
        GrandTotalAmount = grandTotalAmount;
        ExpectedDeliveryDate = expectedDeliveryDate;
        Note = note;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private PartnerProductQuotation() : base()
    {
    }

    public static ResultT<PartnerProductQuotation> Create(
        string code,
        PartnerProductRequestId partnerProductRequestId,
        int version,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal importTaxAmount,
        DateTime expectedDeliveryDate,
        string? note = null,
        int status = 0,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<PartnerProductQuotation>(PartnerProductQuotationError.InvalidCode());

        if (subTotalAmount < 0)
            return Result.Failure<PartnerProductQuotation>(PartnerProductQuotationError.InvalidAmount());

        var grandTotalAmount = subTotalAmount + shippingFee + importTaxAmount;

        var quotationId = PartnerProductQuotationId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var quotation = new PartnerProductQuotation(
            quotationId,
            code,
            partnerProductRequestId,
            version,
            subTotalAmount,
            shippingFee,
            importTaxAmount,
            grandTotalAmount,
            expectedDeliveryDate,
            note,
            status,
            timestamp);

        return Result.Success(quotation);
    }

    public Result UpdateStatus(int status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
