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

    public static Error InvalidStatusTransition(SupportTicketStatus currentStatus, SupportTicketStatus newStatus) =>
        Error.Validation("SupportTicket.InvalidStatusTransition", $"Cannot transition from {currentStatus} to {newStatus}");

    public static Error UnauthorizedStatusTransition() =>
        Error.Forbidden("SupportTicket.UnauthorizedStatusTransition", "You are not authorized to update the support ticket status to this value");

    public static Error StatusAlreadySet(SupportTicketStatus status) =>
        Error.Validation("SupportTicket.StatusAlreadySet", $"Support ticket status is already {status}");

    public static Error PartNotFound(Guid partId) =>
        Error.NotFound("SupportTicket.PartNotFound", $"Part with ID {partId} not found");

    public static Error ReplacePartQuantityExceedsAvailable(Guid partId, int availableQuantity, int requestedQuantity) =>
        Error.Validation("SupportTicket.ReplacePartQuantityExceedsAvailable", $"Requested quantity ({requestedQuantity}) exceeds available part quantity ({availableQuantity}) for part {partId}");

    public static Error CannotCreateNewTicketWithUnresolvedTickets() =>
        Error.Validation("SupportTicket.CannotCreateNewTicket", "Cannot create a new support ticket while there are unresolved support tickets for this order");

    public static Error OrderStatusNotEligibleForSupportTicket(Guid orderId, string currentStatus) =>
        Error.Validation("SupportTicket.OrderStatusNotEligible", $"Support ticket can only be created when order {orderId} status is 'HandedOverToDelivery', but current status is '{currentStatus}'");
}
