using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;

public static class SupportTicketDetailError
{
    public static Error InvalidOrderItemId() =>
        Error.Validation("SupportTicketDetail.InvalidOrderItemId", "Order Item ID cannot be empty");

    public static Error InvalidPartId() =>
        Error.Validation("SupportTicketDetail.InvalidPartId", "Part ID cannot be empty");

    public static Error InvalidQuantity() =>
        Error.Validation("SupportTicketDetail.InvalidQuantity", "Quantity must be greater than 0");

    public static Error NoteTooLong() =>
        Error.Validation("SupportTicketDetail.NoteTooLong", "Note cannot exceed 500 characters");
}
