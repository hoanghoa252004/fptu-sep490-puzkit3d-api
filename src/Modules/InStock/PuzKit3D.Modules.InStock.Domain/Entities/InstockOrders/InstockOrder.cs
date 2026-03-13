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
    public string CustomerProvinceCode { get; private set; } = null!;
    public string CustomerProvinceName { get; private set; } = null!;
    public string CustomerDistrictCode { get; private set; } = null!;
    public string CustomerDistrictName { get; private set; } = null!;
    public string CustomerWardCode { get; private set; } = null!;
    public string CustomerWardName { get; private set; } = null!;
    public decimal SubTotalAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public int UsedCoinAmount { get; private set; }
    public decimal UsedCoinAmountAsMoney { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public InstockOrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public bool IsPaid { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public IReadOnlyCollection<InstockOrderDetail> OrderDetails => _orderDetails.AsReadOnly();

    private InstockOrder(
        InstockOrderId id,
        string code,
        Guid customerId,
        string customerName,
        string customerPhone,
        string customerEmail,
        string customerProvinceCode,
        string customerProvinceName,
        string customerDistrictCode,
        string customerDistrictName,
        string customerWardCode,
        string customerWardName,
        decimal subTotalAmount,
        decimal shippingFee,
        int usedCoinAmount,
        decimal usedCoinAmountAsMoney,
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
        CustomerProvinceCode = customerProvinceCode;
        CustomerProvinceName = customerProvinceName;
        CustomerDistrictCode = customerDistrictCode;
        CustomerDistrictName = customerDistrictName;
        CustomerWardCode = customerWardCode;
        CustomerWardName = customerWardName;
        SubTotalAmount = subTotalAmount;
        ShippingFee = shippingFee;
        UsedCoinAmount = usedCoinAmount;
        UsedCoinAmountAsMoney = usedCoinAmountAsMoney;
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
        string customerProvinceCode,
        string customerProvinceName,
        string customerDistrictCode,
        string customerDistrictName,
        string customerWardCode,
        string customerWardName,
        decimal subTotalAmount,
        decimal shippingFee,
        int usedCoinAmount,
        decimal usedCoinAmountAsMoney,
        decimal grandTotalAmount,
        string paymentMethod,
        InstockOrderStatus status = InstockOrderStatus.PaymentPending,
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

        if (string.IsNullOrWhiteSpace(customerProvinceCode) || string.IsNullOrWhiteSpace(customerProvinceName))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAddress());

        if (string.IsNullOrWhiteSpace(customerDistrictCode) || string.IsNullOrWhiteSpace(customerDistrictName))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAddress());

        if (string.IsNullOrWhiteSpace(customerWardCode) || string.IsNullOrWhiteSpace(customerWardName))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAddress());

        if (string.IsNullOrWhiteSpace(paymentMethod))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidPaymentMethod());

        if (!paymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase) && !paymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase))
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidPaymentMethod());

        if (subTotalAmount < 0 || shippingFee < 0 || usedCoinAmountAsMoney < 0 || grandTotalAmount < 0)
            return Result.Failure<InstockOrder>(InstockOrderError.InvalidAmount());

        var orderId = InstockOrderId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var order = new InstockOrder(
            orderId,
            code,
            customerId,
            customerName,
            customerPhone,
            customerEmail,
            customerProvinceCode,
            customerProvinceName,
            customerDistrictCode,
            customerDistrictName,
            customerWardCode,
            customerWardName,
            subTotalAmount,
            shippingFee,
            usedCoinAmount,
            usedCoinAmountAsMoney,
            grandTotalAmount,
            status,
            paymentMethod,
            isPaid,
            timestamp);

        return Result.Success(order);
    }

    public Result UpdateStatus(InstockOrderStatus newStatus)
    {
        if (!InstockOrderStatusTransition.IsValidTransition(Status, newStatus))
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, newStatus));
        }

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result MarkAsPaid()
    {
        if (Status == InstockOrderStatus.Expired)
        {
            return Result.Failure(InstockOrderError.OrderExpired());
        }

        if (IsPaid)
        {
            return Result.Failure(InstockOrderError.OrderAlreadyPaid());
        }

        if (Status != InstockOrderStatus.PaymentPending)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Paid));
        }

        Status = InstockOrderStatus.Paid;
        IsPaid = true;
        PaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result MarkAsExpired()
    {
        if (Status != InstockOrderStatus.PaymentPending)
        {
            return Result.Failure(InstockOrderError.CannotExpireOrder());
        }

        Status = InstockOrderStatus.Expired;
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

    public Result MarkAsShipped()
    {
        if (Status != InstockOrderStatus.Processing)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Shipped));
        }

        Status = InstockOrderStatus.Shipped;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != InstockOrderStatus.Shipped)
        {
            return Result.Failure(InstockOrderError.InvalidStatusTransition(Status, InstockOrderStatus.Completed));
        }

        if (Status == InstockOrderStatus.Completed)
        {
            return Result.Failure(InstockOrderError.OrderAlreadyCompleted());
        }

        Status = InstockOrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

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
            PaidAt));
    }
}
