using PuzKit3D.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
public sealed class OrderDetailReplica : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? VariantId { get; private set; }
    public int Quantity { get; private set; }

    private OrderDetailReplica(
        Guid id,
        Guid orderId,
        Guid productId,
        Guid? variantId,
        int quantity) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        VariantId = variantId;
        Quantity = quantity;
    }

    private OrderDetailReplica() : base()
    {
    }

    public static OrderDetailReplica Create(
        Guid id,
        Guid orderId,
        Guid productId,
        Guid? variantId,
        int quantity)
    {
        return new OrderDetailReplica(
            id,
            orderId,
            productId,
            variantId,
            quantity);
    }
}
