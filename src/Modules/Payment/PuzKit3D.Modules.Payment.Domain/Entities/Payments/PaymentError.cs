using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Domain.Entities.Payments;

public static class PaymentError
{
    public static Error InvalidReferenceOrderId() => 
        Error.Validation("Payment.InvalidReferenceOrderId", "Reference order ID cannot be empty.");

    public static Error InvalidReferenceOrderType() =>
        Error.Validation("Payment.InvalidReferenceOrderType", "Reference order type is required and cannot exceed 30 characters.");

    public static Error InvalidAmount() =>
        Error.Validation("Payment.InvalidAmount", "Amount must be greater than 0.");

    public static Error InvalidPaymentMethod() =>
        Error.Validation("Payment.InvalidPaymentMethod", "Payment method is required and invalid.");

    public static Error InvalidOperationOnPaymentMethod() =>
        Error.Validation("Payment.InvalidPaymentMethod", "Can not create transation ( online payment ) for COD payment");

    public static Error InvalidStatus() =>
        Error.Validation("Payment.InvalidStatus", "Payment status is invalid.");

    public static Error OrderNotFound(Guid orderId) =>
        Error.NotFound("Payment.OrderNotFound", $"Order with ID '{orderId}' was not found.");

    public static Error PaymentNotFound(Guid orderId) =>
        Error.NotFound("Payment.PaymentNotFound", $"Payment for order ID '{orderId}' was not found.");

    public static Error PaymentExpired() =>
        Error.Validation("Payment.PaymentExpired", "Payment has expired.");

    public static Error PaymentAlreadyPaid() =>
        Error.Validation("Payment.PaymentAlreadyPaid", "Payment has already been paid.");

    public static Error UnauthorizedAccessToOrder() =>
        Error.Forbidden("Payment.UnauthorizedAccessToOrder", "You do not have permission to create a payment URL for this order.");

    public static Error UnsupportedPaymentProvider(string provider) =>
        Error.Validation("Payment.UnsupportedPaymentProvider", $"Payment provider '{provider}' is not supported.");

    public static Error ActiveTransactionExists() =>
        Error.Validation("Payment.ActiveTransactionExists", "An active payment transaction already exists. Please wait for it to expire before creating a new one.");

    public static Error PaymentConfigNotFound() =>
        Error.NotFound("Payment.PaymentConfigNotFound", "Payment configuration was not found.");

    public static Error InvalidRange() =>
        Error.Validation("Payment.InvalidRange", "Time must be > 0");

    public static Error InvalidOnlinePaymentExpiredInDays() =>
        Error.Validation("Payment.InvalidOnlinePaymentExpiredInDays", "Online payment expiration must be at least 1 and not > 10.");

    public static Error InvalidOnlineTransactionExpiredInMinutes() =>
        Error.Validation("Payment.InvalidOnlineTransactionExpiredInMinutes", "Online transaction expiration must be at least 5 minutes and not > 60 minutes.");
}
