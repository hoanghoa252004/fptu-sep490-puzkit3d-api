using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public class PartnerProductOrder : AggregateRoot<PartnerProductOrderId>
{
    public string Code { get; private set; } = null!;
    public PartnerProductQuotationId PartnerProductQuotationId { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; } = null!;
    public string CustomerPhone { get; private set; } = null!;
    public string CustomerEmail { get; private set; } = null!;
    public string CustomerProvinceName { get; private set; } = null!;
    public string CustomerDistrictName { get; private set; } = null!;
    public string CustomerWardName { get; private set; } = null!;
    public string DetailAddress { get; private set; } = null!;
    public decimal SubTotalAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal BaseShippingFee { get; private set; }
    public int UsedCoinAmount { get; private set; }
    public decimal ImportTaxAmount { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public PartnerProductOrderStatus Status { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public bool IsPaid { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    private readonly List<PartnerProductOrderDetail> _details = new();
    public IReadOnlyList<PartnerProductOrderDetail> Details => _details;

    private PartnerProductOrder(
        PartnerProductOrderId id,
        string code,
        PartnerProductQuotationId partnerProductQuotationId,
        Guid customerId,
        string customerName,
        string customerPhone,
        string customerEmail,
        string customerProvinceName,
        string customerDistrictName,
        string customerWardName,
        string detailAddress,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal baseShippingFee,
        decimal importTaxAmount,
        int usedCoinAmount,
        decimal grandTotalAmount,
        PartnerProductOrderStatus status,
        string paymentMethod,
        bool isPaid,
        DateTime createdAt) : base(id)
    {
        Code = code;
        PartnerProductQuotationId = partnerProductQuotationId;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerPhone = customerPhone;
        CustomerEmail = customerEmail;
        CustomerProvinceName = customerProvinceName;
        CustomerDistrictName = customerDistrictName;
        CustomerWardName = customerWardName;
        DetailAddress = detailAddress;
        SubTotalAmount = subTotalAmount;
        ShippingFee = shippingFee;
        BaseShippingFee = baseShippingFee;
        ImportTaxAmount = importTaxAmount;
        UsedCoinAmount = usedCoinAmount;
        GrandTotalAmount = grandTotalAmount;
        Status = status;
        PaymentMethod = paymentMethod;
        IsPaid = isPaid;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private PartnerProductOrder() : base()
    {
    }

    public static ResultT<PartnerProductOrder> Create(
        string code,
        PartnerProductQuotationId partnerProductQuotationId,
        Guid customerId,
        string customerName,
        string customerPhone,
        string customerEmail,
        string customerProvinceName,
        string customerDistrictName,
        string customerWardName,
        string detailAddress,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal baseShippingFee,
        decimal importTaxAmount,
        int usedCoinAmount,
        decimal grandTotalAmount,
        string paymentMethod,
        bool isPaid = false,
        DateTime? createdAt = null)

    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidCode());

        if (string.IsNullOrWhiteSpace(customerName))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidCustomerName());

        if (string.IsNullOrWhiteSpace(customerPhone))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidCustomerPhone());

        if (string.IsNullOrWhiteSpace(customerEmail))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidCustomerEmail());

        if (string.IsNullOrWhiteSpace(customerProvinceName)
            || string.IsNullOrWhiteSpace(customerDistrictName)
            || string.IsNullOrWhiteSpace(customerWardName)
            || string.IsNullOrWhiteSpace(detailAddress))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidAddress());

        if (string.IsNullOrWhiteSpace(paymentMethod))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidPaymentMethod());

        if (!paymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase)
             && !paymentMethod.Equals("COIN", StringComparison.OrdinalIgnoreCase))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidPaymentMethod());

        if (subTotalAmount < 0
            || shippingFee < 0
            || baseShippingFee < 0
            || importTaxAmount < 0
            || usedCoinAmount < 0
            || grandTotalAmount < 0)
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidAmount());

        var orderId = PartnerProductOrderId.Create();
        var time = createdAt ?? DateTime.UtcNow;

        string finalPaymentMethod;
        PartnerProductOrderStatus initialStatus;
        bool finalIsPaid;
        DateTime? finalPaidAt = null;

        if (grandTotalAmount == 0)
        {
            finalPaymentMethod = "COIN";
            initialStatus = PartnerProductOrderStatus.Paid;
            finalIsPaid = true;
            finalPaidAt = time;
        }
        else
        {
            finalPaymentMethod = paymentMethod;
            initialStatus = PartnerProductOrderStatus.Pending;
            finalIsPaid = isPaid;
            finalPaidAt = null;
        }

        var order = new PartnerProductOrder(
            orderId,
            code,
            partnerProductQuotationId,
            customerId,
            customerName,
            customerPhone,
            customerEmail,
            customerProvinceName,
            customerDistrictName,
            customerWardName,
            detailAddress,
            subTotalAmount,
            shippingFee,
            baseShippingFee,
            importTaxAmount,
            usedCoinAmount,
            grandTotalAmount,
            initialStatus,
            finalPaymentMethod,
            finalIsPaid,
            time);

        if (finalPaidAt.HasValue)
        {
            order.PaidAt = finalPaidAt.Value;
        }

        // Raise domain event for coin usage if coins were used
        if (usedCoinAmount > 0)
        {
            order.RaiseDomainEvent(new CoinUsedDomainEvent(
                orderId.Value,
                code,
                customerId,
                usedCoinAmount,
                time));
        }

        return Result.Success(order);
    }

    public Result UpdateStatus(PartnerProductOrderStatus newStatus)
    {
        if (Status == newStatus)
            return Result.Failure(PartnerProductOrderError.InvalidStatusTransition(Status, newStatus));

        if (!PartnerProductOrderStatusTransition.IsValidTransition(Status, newStatus))
            return Result.Failure(PartnerProductOrderError.InvalidStatusTransition(Status, newStatus));

        var oldStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Raise status changed event
        RaiseStatusChangedEvent();

        // Raise domain event for wallet refund - customer
        bool hasPayment = IsPaid || UsedCoinAmount > 0;
        if (Status == PartnerProductOrderStatus.CancelledByCustomer && hasPayment)
        {
            bool isBeforeOrdering = oldStatus < PartnerProductOrderStatus.OrderedFromPartner;
            bool isCheckingFailed = oldStatus == PartnerProductOrderStatus.CheckingFailed;

            if (isBeforeOrdering || isCheckingFailed)
            {
                RaiseDomainEvent(new PartnerProductOrderRefundedDomainEvent(
                Id.Value,
                Code,
                CustomerId,
                GrandTotalAmount,
                UsedCoinAmount,
                PaymentMethod,
                1m,
                UpdatedAt));
            }
        }

        // Raise domain event for wallet refund - staff
        if (Status == PartnerProductOrderStatus.CancelledByStaff
            && oldStatus >= PartnerProductOrderStatus.Paid
            && hasPayment)
        {
            RaiseDomainEvent(new PartnerProductOrderRefundedDomainEvent(
                Id.Value,
                Code,
                CustomerId,
                GrandTotalAmount,
                UsedCoinAmount,
                PaymentMethod,
                1.2m,
                UpdatedAt));
        }

        return Result.Success();
    }

    public Result MarkAsPaid(DateTime paidAt)
    {
        if (Status == PartnerProductOrderStatus.Expired)
            return Result.Failure(PartnerProductOrderError.OrderExpired());
        if (IsPaid)
            return Result.Failure(PartnerProductOrderError.AlreadyPaid());

        if (string.Equals(PaymentMethod, "Online", StringComparison.OrdinalIgnoreCase))
        {
            if (Status != PartnerProductOrderStatus.Pending)
            {
                return Result.Failure(PartnerProductOrderError.InvalidStatusTransition(Status, PartnerProductOrderStatus.Paid));
            }
            Status = PartnerProductOrderStatus.Paid;
        }
        IsPaid = true;
        PaidAt = paidAt;
        UpdatedAt = paidAt;

        return Result.Success();
    }

    public void AddDetail(PartnerProductOrderDetail detail)
    {
        _details.Add(detail);
    }

    // Raise domain event after the order is created and all details are added
    public void RaiseOrderCreatedDomainEvent()
    {
        var items = _details.Select(d => new PartnerProductOrderItemInfo(
            d.Id.Value,
            d.PartnerProductId.Value,
            d.Quantity,
            d.PartnerProductName)).ToList();

        RaiseDomainEvent(new PartnerProductOrderCreatedDomainEvent(
            Id.Value,
            CustomerId,
            Code,
            GrandTotalAmount,
            Status.ToString(),
            PaymentMethod,
            UsedCoinAmount,
            IsPaid,
            PaidAt,
            CreatedAt,
            items));
    }

    private void RaiseStatusChangedEvent()
    {
        RaiseDomainEvent(new PartnerProductOrderStatusUpdatedDomainEvent(
            Id.Value,
            Code,
            CustomerId,
            Status,
            UpdatedAt));
    }
}
