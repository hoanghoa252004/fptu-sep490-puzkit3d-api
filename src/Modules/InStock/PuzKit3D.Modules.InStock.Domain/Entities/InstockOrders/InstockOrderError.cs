using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public static class InstockOrderError
{
    public static Error InvalidCode() => Error.Validation(
        "InstockOrder.InvalidCode",
        "Order code cannot be empty.");

    public static Error InvalidCustomerName() => Error.Validation(
        "InstockOrder.InvalidCustomerName",
        "Customer name cannot be empty.");

    public static Error InvalidCustomerPhone() => Error.Validation(
        "InstockOrder.InvalidCustomerPhone",
        "Customer phone cannot be empty.");

    public static Error InvalidCustomerEmail() => Error.Validation(
        "InstockOrder.InvalidCustomerEmail",
        "Customer email cannot be empty.");

    public static Error InvalidAddress() => Error.Validation(
        "InstockOrder.InvalidAddress",
        "Address information cannot be empty.");

    public static Error InvalidAmount() => Error.Validation(
        "InstockOrder.InvalidAmount",
        "Amount must be greater than or equal to zero.");

    public static Error InvalidPaymentMethod() => Error.Validation(
        "InstockOrder.InvalidPaymentMethod",
        "Payment method cannot be empty.");

    public static Error InvalidStatus() => Error.Validation(
        "InstockOrder.InvalidStatus",
        "Order status is invalid.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "InstockOrder.NotFound",
        $"Instock order with ID '{id}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "InstockOrder.DuplicateCode",
        $"Instock order with code '{code}' already exists.");

    public static Error CannotModifyOrder() => Error.Validation(
        "InstockOrder.CannotModifyOrder",
        "Order cannot be modified in current status.");
}
