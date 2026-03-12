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

    public static Error EmptyCart() => Error.Validation(
        "InstockOrder.EmptyCart",
        "Cannot create order with empty cart.");

    public static Error VariantNotFound(Guid variantId) => Error.NotFound(
        "InstockOrder.VariantNotFound",
        $"Product variant with ID '{variantId}' was not found.");

    public static Error VariantNotActive(string sku) => Error.Validation(
        "InstockOrder.VariantNotActive",
        $"Product variant with SKU '{sku}' is not active.");

    public static Error PriceDetailNotFound(Guid priceDetailId) => Error.NotFound(
        "InstockOrder.PriceDetailNotFound",
        $"Price detail with ID '{priceDetailId}' was not found.");

    public static Error PriceDetailNotActive(Guid priceDetailId) => Error.Validation(
        "InstockOrder.PriceDetailNotActive",
        $"Price detail with ID '{priceDetailId}' is not active.");

    public static Error PriceNotActiveOrNotFound() => Error.Validation(
        "InstockOrder.PriceNotActiveOrNotFound",
        "The price is not active or not found.");

    public static Error PriceNotHighestPriority() => Error.Validation(
        "InstockOrder.PriceNotHighestPriority",
        "The selected price does not have the highest priority among active prices.");

    public static Error GrandTotalMismatch(decimal calculated, decimal provided) => Error.Validation(
        "InstockOrder.GrandTotalMismatch",
        $"Grand total mismatch. Calculated: {calculated}, Provided: {provided}.");

    public static Error InvalidStatusTransition(InstockOrderStatus currentStatus, InstockOrderStatus newStatus) => Error.Validation(
        "InstockOrder.InvalidStatusTransition",
        $"Cannot transition from '{currentStatus}' to '{newStatus}'. Allowed transitions: {InstockOrderStatusTransition.GetTransitionPath()}");

    public static Error OrderAlreadyPaid() => Error.Validation(
        "InstockOrder.OrderAlreadyPaid",
        "Order has already been paid.");

    public static Error OrderNotPaid() => Error.Validation(
        "InstockOrder.OrderNotPaid",
        "Order must be paid before processing.");

    public static Error OrderExpired() => Error.Validation(
        "InstockOrder.OrderExpired",
        "Order has expired and cannot be modified.");

    public static Error OrderAlreadyCompleted() => Error.Validation(
        "InstockOrder.OrderAlreadyCompleted",
        "Order has already been completed.");

    public static Error CannotExpireOrder() => Error.Validation(
        "InstockOrder.CannotExpireOrder",
        "Only orders in PaymentPending status can be expired.");
}

