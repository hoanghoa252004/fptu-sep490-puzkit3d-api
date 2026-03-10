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
    public string CustomerProvinceCode { get; private set; } = null!;
    public string CustomerProvinceName { get; private set; } = null!;
    public string CustomerDistrictCode { get; private set; } = null!;
    public string CustomerDistrictName { get; private set; } = null!;
    public string CustomerWardCode { get; private set; } = null!;
    public string CustomerWardName { get; private set; } = null!;
    public decimal SubTotalAmount { get; private set; }
    public decimal ShippingFee { get; private set; }
    public decimal ImportTaxAmount { get; private set; }
    public decimal UsedCoinAmountAsMoney { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public int Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string PaymentMethod { get; private set; } = "ONLINE";
    public bool IsPaid { get; private set; }
    public DateTime? PaidAt { get; private set; }

    private PartnerProductOrder(
        PartnerProductOrderId id,
        string code,
        PartnerProductQuotationId partnerProductQuotationId,
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
        decimal importTaxAmount,
        decimal usedCoinAmountAsMoney,
        decimal grandTotalAmount,
        int status,
        string paymentMethod,
        bool isPaid,
        DateTime? paidAt,
        DateTime createdAt) : base(id)
    {
        Code = code;
        PartnerProductQuotationId = partnerProductQuotationId;
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
        ImportTaxAmount = importTaxAmount;
        UsedCoinAmountAsMoney = usedCoinAmountAsMoney;
        GrandTotalAmount = grandTotalAmount;
        Status = status;
        PaymentMethod = paymentMethod;
        IsPaid = isPaid;
        PaidAt = paidAt;
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
        string customerProvinceCode,
        string customerProvinceName,
        string customerDistrictCode,
        string customerDistrictName,
        string customerWardCode,
        string customerWardName,
        decimal subTotalAmount,
        decimal shippingFee,
        decimal importTaxAmount,
        decimal usedCoinAmountAsMoney,
        string paymentMethod = "ONLINE",
        int status = 0,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidCode());

        if (subTotalAmount < 0)
            return Result.Failure<PartnerProductOrder>(PartnerProductOrderError.InvalidAmount());

        var grandTotalAmount = subTotalAmount + shippingFee + importTaxAmount - usedCoinAmountAsMoney;

        var orderId = PartnerProductOrderId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var order = new PartnerProductOrder(
            orderId,
            code,
            partnerProductQuotationId,
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
            importTaxAmount,
            usedCoinAmountAsMoney,
            grandTotalAmount,
            status,
            paymentMethod,
            false,
            null,
            timestamp);

        return Result.Success(order);
    }

    public Result UpdateStatus(int status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result MarkAsPaid()
    {
        if (IsPaid)
            return Result.Failure(PartnerProductOrderError.AlreadyPaid());

        IsPaid = true;
        PaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
