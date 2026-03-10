using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

public class PartnerProductRequest : AggregateRoot<PartnerProductRequestId>
{
    public string Code { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public PartnerId PartnerId { get; private set; } = null!;
    public DateTime DesiredDeliveryDate { get; private set; }
    public int TotalRequestedQuantity { get; private set; }
    public string? Note { get; private set; }
    public int Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PartnerProductRequest(
        PartnerProductRequestId id,
        string code,
        Guid customerId,
        PartnerId partnerId,
        DateTime desiredDeliveryDate,
        int totalRequestedQuantity,
        string? note,
        int status,
        DateTime createdAt) : base(id)
    {
        Code = code;
        CustomerId = customerId;
        PartnerId = partnerId;
        DesiredDeliveryDate = desiredDeliveryDate;
        TotalRequestedQuantity = totalRequestedQuantity;
        Note = note;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private PartnerProductRequest() : base()
    {
    }

    public static ResultT<PartnerProductRequest> Create(
        string code,
        Guid customerId,
        PartnerId partnerId,
        DateTime desiredDeliveryDate,
        int totalRequestedQuantity,
        string? note = null,
        int status = 0,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<PartnerProductRequest>(PartnerProductRequestError.InvalidCode());

        if (totalRequestedQuantity <= 0)
            return Result.Failure<PartnerProductRequest>(PartnerProductRequestError.InvalidQuantity());

        var requestId = PartnerProductRequestId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var request = new PartnerProductRequest(
            requestId,
            code,
            customerId,
            partnerId,
            desiredDeliveryDate,
            totalRequestedQuantity,
            note,
            status,
            timestamp);

        return Result.Success(request);
    }

    public Result UpdateStatus(int status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateQuantity(int totalRequestedQuantity)
    {
        if (totalRequestedQuantity <= 0)
            return Result.Failure(PartnerProductRequestError.InvalidQuantity());

        TotalRequestedQuantity = totalRequestedQuantity;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
