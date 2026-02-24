using PuzKit3D.Modules.InStock.Domain.Events.Orders;
using PuzKit3D.Modules.InStock.Domain.ValueObjects;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Orders;

public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _orderItems = new();

    public Guid UserId { get; private set; }
    public Money TotalMoney { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order(OrderId id, Guid userId, DateTime createdAt) : base(id)
    {
        UserId = userId;
        CreatedAt = createdAt;
        TotalMoney = Money.Zero();
    }

    private Order() : base()
    {
    }

    public static ResultT<Order> Create(Guid userId, DateTime createdAt)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Order>(OrderError.InvalidUserId());

        var orderId = OrderId.Create();
        var order = new Order(orderId, userId, createdAt);

        return Result.Success(order);
    }

    public Result AddOrderItem(OrderItem orderItem)
    {
        if (orderItem == null)
            return Result.Failure(OrderError.InvalidOrderItem());

        _orderItems.Add(orderItem);
        RecalculateTotalMoney();

        return Result.Success();
    }

    public void CompleteOrder()
    {
        RaiseDomainEvent(new OrderCreatedDomainEvent(
            Id.Value,
            UserId,
            TotalMoney.Amount,
            CreatedAt));
    }

    private void RecalculateTotalMoney()
    {
        TotalMoney = _orderItems.Aggregate(
            Money.Zero(),
            (current, item) => current.Add(item.TotalPrice));
    }
}
