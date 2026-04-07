using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;

public static class MilestoneQuotationDetailError
{
    public static Error InvalidLaborCost() => Error.Validation(
        "MilestoneQuotationDetail.InvalidLaborCost",
        "Labor cost cannot be negative.");

    public static Error InvalidWeightPercent() => Error.Validation(
        "MilestoneQuotationDetail.InvalidWeightPercent",
        "Weight percent must be between 0 and 100.");

    public static Error InvalidWeightAmount() => Error.Validation(
        "MilestoneQuotationDetail.InvalidWeightAmount",
        "Weight amount cannot be negative.");

    public static Error InvalidTotalAmount() => Error.Validation(
        "MilestoneQuotationDetail.InvalidTotalAmount",
        "Total amount cannot be negative.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "MilestoneQuotationDetail.NotFound",
        $"Milestone quotation detail with ID '{id}' was not found.");
}
