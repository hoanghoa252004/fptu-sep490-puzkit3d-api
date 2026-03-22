using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;

public static class SupportTicketError
{
    public static Error InvalidUserId() =>
        Error.Validation("SupportTicket.InvalidUserId", "User ID cannot be empty");

    public static Error InvalidOrderId() =>
        Error.Validation("SupportTicket.InvalidOrderId", "Order ID cannot be empty");

    public static Error InvalidType() =>
        Error.Validation("SupportTicket.InvalidType", "Invalid support ticket type");

    public static Error InvalidReason() =>
        Error.Validation("SupportTicket.InvalidReason", "Reason cannot be empty");

    public static Error ReasonTooLong() =>
        Error.Validation("SupportTicket.ReasonTooLong", "Reason cannot exceed 1000 characters");

    public static Error InvalidProof() =>
        Error.Validation("SupportTicket.InvalidProof", "Proof cannot be empty");

    public static Error ProofTooLong() =>
        Error.Validation("SupportTicket.ProofTooLong", "Proof path cannot exceed 500 characters");

    public static Error DetailsRequiredForReplacePart() =>
        Error.Validation("SupportTicket.DetailsRequiredForReplacePart", "At least one detail is required for ReplacePart type");

    public static Error PartIdRequiredForReplacePart() =>
        Error.Validation("SupportTicket.PartIdRequiredForReplacePart", "PartId is required when type is ReplacePart");

    public static Error ExchangeQuantityExceedsOrderDetailQuantity(Guid orderDetailId, int orderDetailQuantity) =>
        Error.Validation("SupportTicket.ExchangeQuantityExceedsOrderDetailQuantity", $"Exchange quantity cannot exceed order detail quantity ({orderDetailQuantity}) for order detail {orderDetailId}");

    public static Error CanOnlyDeleteOpenTickets() =>
        Error.Validation("SupportTicket.CanOnlyDeleteOpenTickets", "Can only delete support tickets with Open status");

    public static Error Unauthorized() =>
        Error.Forbidden("SupportTicket.Unauthorized", "You can only delete your own support tickets");
}
