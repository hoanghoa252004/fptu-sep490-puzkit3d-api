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

    public static Error InvalidProvider(int maxLength) =>
        Error.Validation("Payment.InvalidProvider", $"Provider cannot exceed {maxLength} characters.");

    public static Error InvalidStatus() =>
        Error.Validation("Payment.InvalidStatus", "Payment status is invalid.");

    public static Error OrderNotFound(Guid orderId) =>
        Error.NotFound("Payment.OrderNotFound", $"Order with ID '{orderId}' was not found.");

    public static Error PaymentExpired() =>
        Error.Validation("Payment.PaymentExpired", "Payment has expired.");

    public static Error PaymentAlreadyPaid() =>
        Error.Validation("Payment.PaymentAlreadyPaid", "Payment has already been paid.");
}

