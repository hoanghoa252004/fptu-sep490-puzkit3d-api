using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Orders;

public sealed class OrderError
{
    public static Error InvalidUserId() =>
        Error.Failure(
            "Order.InvalidUserId",
            "User Id cannot be empty");

    public static Error InvalidOrderItem() =>
        Error.Failure(
            "Order.InvalidOrderItem",
            "Order item cannot be null");

    public static Error NotFound(Guid orderId) =>
        Error.NotFound(
            "Order.NotFound",
            $"Order with Id [{orderId}] was not found");
}
