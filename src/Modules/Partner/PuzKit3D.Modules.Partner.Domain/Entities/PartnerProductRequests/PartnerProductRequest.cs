using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests.DomainEvents;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
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

    private readonly List<PartnerProductRequestDetail> _details = new();
    public IReadOnlyList<PartnerProductRequestDetail> Details => _details;

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
        List<(PartnerProductId productId, int quantity, decimal price)> items,
        string? note = null,
        int status = 0,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<PartnerProductRequest>(PartnerProductRequestError.InvalidCode());

        if (items == null || !items.Any())
            return Result.Failure<PartnerProductRequest>(PartnerProductRequestError.InvalidQuantity());

        var requestId = PartnerProductRequestId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var totalQuantity = items.Sum(x => x.quantity);

        var request = new PartnerProductRequest(
            requestId,
            code,
            customerId,
            partnerId,
            desiredDeliveryDate,
            totalQuantity,
            note,
            status,
            timestamp);

        foreach (var item in items)
        {
            var detailResult = PartnerProductRequestDetail.Create(
                requestId,
                item.productId,
                item.price,
                item.quantity);

            if (detailResult.IsFailure)
                return Result.Failure<PartnerProductRequest>(detailResult.Error);

            request._details.Add(detailResult.Value);
        }

        request.RaiseDomainEvent(new PartnerProductRequestCreatedDomainEvent(
            requestId.Value,
            customerId,
            partnerId.Value,
            request.Details.Select(d => new PartnerProductRequestDetailDto
            (
                d.PartnerProductId.Value,
                d.Quantity,
                d.ReferenceUnitPrice
            )).ToList(),
            timestamp));

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
