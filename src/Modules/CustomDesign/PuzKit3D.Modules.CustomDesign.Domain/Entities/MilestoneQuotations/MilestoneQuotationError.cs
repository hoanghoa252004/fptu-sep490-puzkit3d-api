using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;

public static class MilestoneQuotationError
{
    public static Error InvalidCode() => Error.Validation(
        "MilestoneQuotation.InvalidCode",
        "Milestone quotation code cannot be empty.");

    public static Error InvalidTotalAmount() => Error.Validation(
        "MilestoneQuotation.InvalidTotalAmount",
        "Milestone quotation total amount cannot be negative.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "MilestoneQuotation.NotFound",
        $"Milestone quotation with ID '{id}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "MilestoneQuotation.DuplicateCode",
        $"Milestone quotation with code '{code}' already exists.");
}
