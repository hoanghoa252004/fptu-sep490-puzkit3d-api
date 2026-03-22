using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    Guid CustomerId,
    List<Guid> CartItemIds,
    string Code,
    decimal GrandTotalAmount,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime? PaidAt,
    DateTime CreatedAt,
    List<OrderDetail> OrderDetails) : IIntegrationEvent;

public sealed record OrderDetail(
    Guid OrderDetailId,
    Guid VariantId,
    Guid ProductId,
    int Quantity);