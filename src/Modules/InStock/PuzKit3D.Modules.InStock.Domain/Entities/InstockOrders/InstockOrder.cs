using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public sealed class InstockOrder : AggregateRoot<InstockOrderId>
{
    private readonly List<InstockOrderDetail> _orderDetails = new();

    public string Code { get; private set; } = null!;
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
    public int UsedCoinAmount { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public InstockOrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public bool IsPaid { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? MustCompleteBefore { get; private set; }
    //public string? DeliveryOrderCode { get; private set; }
    //public DateTime? ExpectedDeliveryDate { get; private set; }
    //public string? HandoverProofImageUrl { get; private set; }

    public IReadOnlyCollection<InstockOrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private InstockOrder(
        InstockOrderId id,
        string code,
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
        int usedCoinAmount,
        decimal grandTotalAmount,
        InstockOrderStatus status,
        string paymentMethod,
        bool isPaid,
        DateTime createdAt) : base(id)
    {
        Code = code;
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
        UsedCoinAmount = usedCoinAmount;
        GrandTotalAmount = grandTotalAmount;
        Status = status;
        PaymentMethod = paymentMethod;
        IsPaid = isPaid;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockOrder() : base()
    {
    }

    public static ResultT<InstockOrder> Create(
        string code,
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
        int usedCoinAmount,
        decimal grandTotalAmount,
        string paymentMethod,
        bool isPaid = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidCode());

        if (string.IsNullOrWhiteSpace(customerName))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidCustomerName());

        if (string.IsNullOrWhiteSpace(customerPhone))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidCustomerPhone());

        if (string.IsNullOrWhiteSpace(customerEmail))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidCustomerEmail());

        if (string.IsNullOrWhiteSpace(customerProvinceName)
            || string.IsNullOrWhiteSpace(customerDistrictName)
            || string.IsNullOrWhiteSpace(customerWardName)
            || string.IsNullOrWhiteSpace(detailAddress))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAddress());

        if (string.IsNullOrWhiteSpace(paymentMethod))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidPaymentMethod());

        if (!paymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase) && !paymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase)
             && !paymentMethod.Equals("COIN", StringComparison.OrdinalIgnoreCase))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidPaymentMethod());

        if (subTotalAmount < 0 || shippingFee < 0 || usedCoinAmount < 0 || grandTotalAmount < 0)
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAmount());

        var orderId = InstockOrderId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        
        // Determine initial status and payment method based on grand total and payment method
        // If grandTotalAmount = 0, payment is made entirely with coins, so mark as paid immediately with COIN method
        string finalPaymentMethod;
        InstockOrderStatus initialStatus;
        bool finalIsPaid;
        DateTime? finalPaidAt = null;

        if (grandTotalAmount == 0)
        {
            // Payment made entirely with coins
            finalPaymentMethod = "COIN";
            initialStatus = InstockOrderStatus.Paid;
            finalIsPaid = true;
            finalPaidAt = timestamp;
        }
        else
        {
            finalPaymentMethod = paymentMethod;
            finalIsPaid = isPaid;
            finalPaidAt = null;
            
            // Determine initial status based on payment method
            initialStatus = paymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase)
                ? InstockOrderStatus.Waiting
                : InstockOrderStatus.Pending;
        }
        
        var order = new InstockOrder(
            orderId,
            code,
            customerId,
            customerName,
            customerPhone,
            customerEmail,
            customerProvinceName,
            customerDistrictName,
            detailAddress,
            customerWardName,
            subTotalAmount,
            shippingFee,
            usedCoinAmount,
            grandTotalAmount,
            initialStatus,
            finalPaymentMethod,
            finalIsPaid,
            timestamp);

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
                timestamp));
        }

        return Result.Success(order);
    }

    public Result UpdateStatus(InstockOrderStatus newStatus)
    {
        if (Status == newStatus)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, newStatus));
        }

        if (!InstockOrderStatusTransition.IsValidTransition(Status, newStatus))
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, newStatus));
        }

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Raise generic status changed event
        RaiseStatusChangedEvent();

        // Raise specific cancelled event for inventory reversal
        if (Status == InstockOrderStatus.Cancelled)
        {
            var orderDetails = _orderDetails.Select(od => new OrderCancelledDetailInfo(
                od.Id.Value,
                od.InstockProductVariantId.Value,
                od.Quantity))
                .ToList();

            RaiseDomainEvent(new InstockOrderCancelledDomainEvent(
                Id.Value,
                Code,
                CustomerId,
                orderDetails,
                UpdatedAt));
        }

        // Raise domain event for wallet refund
        if (Status == InstockOrderStatus.Cancelled && IsPaid == true || Status == InstockOrderStatus.Cancelled && UsedCoinAmount > 0)
        {
            RaiseDomainEvent(new OrderCancelledRefundCoinDomainEvent(
                Id.Value,
                Code,
                CustomerId,
                GrandTotalAmount,
                UsedCoinAmount,
                PaymentMethod,
                UpdatedAt));
        }

        return Result.Success();
    }

    public Result MarkAsPaid(DateTime paidAt)
    {
        if (Status == InstockOrderStatus.Expired)
        {
            return Result.Failure(InstockOrderError.OrderExpired());
        }

        if (IsPaid)
        {
            return Result.Failure(InstockOrderError.OrderAlreadyPaid());
        }

        if(string.Equals(PaymentMethod, "Online", StringComparison.OrdinalIgnoreCase))
        {
            if (Status != InstockOrderStatus.Pending)
            {
                return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Paid));
            }
            Status = InstockOrderStatus.Paid;
        }

        IsPaid = true;
        PaidAt = paidAt;
        UpdatedAt = paidAt;

        return Result.Success();
    }

    public Result MarkAsExpired()
    {
        if (Status != InstockOrderStatus.Pending && Status != InstockOrderStatus.Waiting)
        {
            return Result.Failure(InstockOrderError.CannotExpireOrder());
        }

        Status = InstockOrderStatus.Expired;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result SetMustCompleteBefore(DateTime mustCompleteBefore)
    {
        MustCompleteBefore = mustCompleteBefore;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result StartProcessing()
    {
        if (Status != InstockOrderStatus.Paid)
        {
            return Result.Failure(InstockOrderError.OrderNotPaid());
        }

        Status = InstockOrderStatus.Processing;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }


    public Result Complete()
    {
        if (Status != InstockOrderStatus.HandedOverToDelivery)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Completed));
        }

        if (Status == InstockOrderStatus.Completed)
        {
            return Result.Failure(InstockOrderError.OrderAlreadyCompleted());
        }

        Status = InstockOrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        //RaiseOrderCompletedEvent();

        return Result.Success();
    }

    public Result MarkAsReturned()
    {
        if (Status == InstockOrderStatus.Returned || Status == InstockOrderStatus.Refunded)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Returned));
        }

        Status = InstockOrderStatus.Returned;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status != InstockOrderStatus.Waiting && Status != InstockOrderStatus.Pending)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Cancelled));
        }

        Status = InstockOrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    //public Result SetDeliveryInfo(string deliveryOrderCode, DateTime expectedDeliveryDate)
    //{
    //    if (string.IsNullOrWhiteSpace(deliveryOrderCode))
    //    {
    //        return Result.Failure(InstockOrderError.InvalidDeliveryOrderCode());
    //    }

    //    if (DeliveryOrderCode != null && ExpectedDeliveryDate != null)
    //    {
    //        return Result.Failure(InstockOrderError.DeliveryInfoAlreadySet());
    //    }

    //    DeliveryOrderCode = deliveryOrderCode;

    //    DateTime utcDateTime;

    //    if (expectedDeliveryDate.Kind == DateTimeKind.Utc)
    //    {
    //        utcDateTime = expectedDeliveryDate;
    //    }
    //    else if (expectedDeliveryDate.Kind == DateTimeKind.Local)
    //    {
    //        utcDateTime = expectedDeliveryDate.ToUniversalTime();
    //    }
    //    else // Unspecified (trường hợp nguy hiểm nhất)
    //    {
    //        var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    //        utcDateTime = TimeZoneInfo.ConvertTimeToUtc(expectedDeliveryDate, vietnamTimeZone);
    //    }

    //    ExpectedDeliveryDate = utcDateTime;
    //    UpdatedAt = DateTime.UtcNow;

    //    return Result.Success();
    //}

    public void AddOrderDetail(InstockOrderDetail orderDetail)
    {
        _orderDetails.Add(orderDetail);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveOrderDetail(InstockOrderDetail orderDetail)
    {
        _orderDetails.Remove(orderDetail);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RaiseOrderCreatedEvent(List<Guid> cartItemIds)
    {
        var orderDetails = _orderDetails.Select(od => new OrderDetailInfo(
            od.Id.Value,
            od.InstockProductVariantId.Value,
            od.Quantity,
            od.ProductName,
            od.VariantName))
            .ToList();

        RaiseDomainEvent(new InstockOrderCreatedDomainEvent(
            Id.Value,
            Code,
            CustomerId,
            cartItemIds,
            GrandTotalAmount,
            CreatedAt,
            Status.ToString(),
            PaymentMethod,
            IsPaid,
            PaidAt,
            orderDetails,
            UsedCoinAmount));
    }

    private void RaiseStatusChangedEvent()
    {
        RaiseDomainEvent(new InstockOrderStatusChangedDomainEvent(
            Id.Value,
            Code,
            CustomerId,
            Status,
            UpdatedAt,
            PaymentMethod,
            GrandTotalAmount,
            UsedCoinAmount));
    }
}
