using PuzKit3D.SharedKernel.Domain.Errors;

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
}
