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
}
