using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.ValueObjects;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;

public sealed class InstockOrderDetail : Entity<InstockOrderDetailId>
{
    public InstockOrderId InstockOrderId { get; private set; } = null!;
    public InstockProductVariantId InstockProductVariantId { get; private set; } = null!;
    public string Sku { get; private set; } = null!;
    public string? ProductName { get; private set; }
    public string? VariantName { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public int Quantity { get; private set; }
    public InstockProductPriceDetailId InstockProductPriceDetailId { get; private set; } = null!;
    public string PriceName { get; private set; } = null!;
    public Money TotalAmount { get; private set; } = null!;

    private InstockOrderDetail(
        InstockOrderDetailId id,
        InstockOrderId instockOrderId,
        InstockProductVariantId instockProductVariantId,
        string sku,
        string? productName,
        string? variantName,
        Money unitPrice,
        int quantity,
        InstockProductPriceDetailId instockProductPriceDetailId,
        string priceName,
        Money totalAmount) : base(id)
    {
        InstockOrderId = instockOrderId;
        InstockProductVariantId = instockProductVariantId;
        Sku = sku;
        ProductName = productName;
        VariantName = variantName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        InstockProductPriceDetailId = instockProductPriceDetailId;
        PriceName = priceName;
        TotalAmount = totalAmount;
    }

    private InstockOrderDetail() : base()
    {
    }

    public static ResultT<InstockOrderDetail> Create(
        InstockOrderId instockOrderId,
        InstockProductVariantId instockProductVariantId,
        string sku,
        decimal unitPrice,
        int quantity,
        InstockProductPriceDetailId instockProductPriceDetailId,
        string priceName,
        string? productName = null,
        string? variantName = null)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return Result.Failure<InstockOrderDetail>(InstockOrderDetailError.InvalidSku());

        if (unitPrice <= 0)
            return Result.Failure<InstockOrderDetail>(InstockOrderDetailError.InvalidUnitPrice());

        if (quantity <= 0)
            return Result.Failure<InstockOrderDetail>(InstockOrderDetailError.InvalidQuantity());

        var orderDetailId = InstockOrderDetailId.Create();
        var unitPriceMoney = Money.Create(unitPrice);
        var totalAmount = unitPriceMoney.Multiply(quantity);

        var orderDetail = new InstockOrderDetail(
            orderDetailId,
            instockOrderId,
            instockProductVariantId,
            sku,
            productName,
            variantName,
            unitPriceMoney,
            quantity,
            instockProductPriceDetailId,
            priceName,
            totalAmount);

        return Result.Success(orderDetail);
    }

    public Result UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(InstockOrderDetailError.InvalidQuantity());

        Quantity = quantity;
        TotalAmount = UnitPrice.Multiply(quantity);

        return Result.Success();
    }
}
