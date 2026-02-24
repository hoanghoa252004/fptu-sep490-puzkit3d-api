using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.Modules.InStock.Domain.ValueObjects;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Orders;

public class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; } = null!;
    public ProductId ProductId { get; private set; } = null!;
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public Money TotalPrice { get; private set; } = null!;

    private OrderItem(
        OrderItemId id,
        OrderId orderId,
        ProductId productId,
        int quantity,
        Money unitPrice) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = unitPrice.Multiply(quantity);
    }

    private OrderItem() : base()
    {
    }

    internal static OrderItem Create(
        OrderId orderId,
        ProductId productId,
        int quantity,
        Money unitPrice)
    {
        return new OrderItem(
            OrderItemId.Create(),
            orderId,
            productId,
            quantity,
            unitPrice);
    }
}
