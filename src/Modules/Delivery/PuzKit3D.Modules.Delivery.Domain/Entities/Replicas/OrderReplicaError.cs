using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
    
public static class OrderReplicaError
{
    public static Error OrderNotFound(Guid orderId) =>
        Error.NotFound("OrderReplica.OrderNotFound", $"Order with ID '{orderId}' was not found .");

    public static Error OrderDetailNotFound(Guid orderId) =>
        Error.NotFound("OrderDetailReplica.OrderDetailNotFound", $"Order detail with ID '{orderId}' was not found .");

}