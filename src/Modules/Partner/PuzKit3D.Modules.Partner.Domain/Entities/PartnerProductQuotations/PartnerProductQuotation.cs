using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

public class PartnerProductQuotation : AggregateRoot<PartnerProductQuotationId>
{
    public string Code { get; private set; } = null!;
    public PartnerProductRequestId PartnerProductRequestId { get; private set; } = null!;
    public decimal SubTotalAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal ImportTaxAmount { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public string? Note { get; private set; }
    public PartnerProductQuotationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<PartnerProductQuotationDetail> _details = new();
    public IReadOnlyList<PartnerProductQuotationDetail> Details => _details;

    private PartnerProductQuotation(
        PartnerProductQuotationId id,
        string code,
        PartnerProductRequestId partnerProductRequestId,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal importTaxAmount,
        decimal grandTotalAmount,
        string? note,
        PartnerProductQuotationStatus status,
        DateTime createdAt) : base(id)
    {
        Code = code;
        PartnerProductRequestId = partnerProductRequestId;
        SubTotalAmount = subTotalAmount;
        ShippingFee = shippingFee;
        ImportTaxAmount = importTaxAmount;
        GrandTotalAmount = grandTotalAmount;
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
            subTotalAmount,
            shippingFee,
            importTaxAmount,
            grandTotalAmount,
            null,
            PartnerProductQuotationStatus.Quoted,
            timestamp);

        return Result.Success(quotation);
    }

    public Result UpdateStatus(PartnerProductQuotationStatus newStatus, string? note = null)
    {
        if (!PartnerProductQuotationStatusTransition.IsValidTransition(Status, newStatus))
        {
            return Result.Failure(
                PartnerProductQuotationError.InvalidStatusTransition(Status, newStatus));
        }
        Status = newStatus;
        if (note != null)
            Note = note;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void AddDetail(PartnerProductQuotationDetail detail)
    {
        _details.Add(detail);
    }
}
