using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;

public static class PartnerProductOrderError
{
    public static Error InvalidCode() => Error.Validation(
        "PartnerProductOrder.InvalidCode",
        "Partner product order code cannot be empty.");

    public static Error InvalidAmount() => Error.Validation(
        "PartnerProductOrder.InvalidAmount",
        "Amount must be greater than or equal to 0.");

    public static Error AlreadyPaid() => Error.Conflict(
        "PartnerProductOrder.AlreadyPaid",
        "Order has already been paid.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "PartnerProductOrder.NotFound",
        $"Partner product order with ID '{id}' was not found.");

    public static Error NotFoundByCode(string code) => Error.NotFound(
        "PartnerProductOrder.NotFoundByCode",
        $"Partner product order with code '{code}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "PartnerProductOrder.DuplicateCode",
        $"Partner product order with code '{code}' already exists.");

    public static Error InvalidStatusTransition(PartnerProductOrderStatus currentStatus, PartnerProductOrderStatus newStatus) => Error.Validation(
        "PartnerProductOrder.InvalidStatusTransition",
        $"Cannot transition from status '{currentStatus}' to '{newStatus}'. This status transition is not allowed.");

    public static Error QuotationNotAccepted(PartnerProductQuotationStatus status) => Error.Validation(
        "PartnerProductOrder.QuotationNotAccepted",
        $"Quotation must be in Accepted status, but current status is '{status}'.");

    public static Error InvalidStatus(string newStatus) => Error.Validation(
        "PartnerProductOrder.InvalidStatus",
        $"'{newStatus}' is not a valid order status.");

    internal static Error InvalidCustomerName() => Error.Validation(
        "PartnerProductOrder.InvalidCustomerName",
        "Customer name cannot be empty.");

    internal static Error InvalidCustomerPhone() => Error.Validation(
        "PartnerProductOrder.InvalidCustomerPhone",
        "Customer phone cannot be empty.");

    internal static Error InvalidCustomerEmail() => Error.Validation(
        "PartnerProductOrder.InvalidCustomerEmail",
        "Customer email cannot be empty.");

    internal static Error InvalidAddress() => Error.Validation(
        "PartnerProductOrder.InvalidAddress",
        "Address information cannot be empty.");

    internal static Error InvalidPaymentMethod() => Error.Validation(
        "PartnerProductOrder.InvalidPaymentMethod",
        "Payment method cannot be empty. Online or COIN");

    internal static Error OrderExpired() => Error.Validation(
        "PartnerProductOrder.OrderExpired",
        "The order has expired and cannot be modified.");

    public static Error AlreadyExistsForQuotation() => Error.Conflict(
        "PartnerProductOrder.AlreadyExistsForQuotation",
        "An order already exists for the given quotation.");
}
