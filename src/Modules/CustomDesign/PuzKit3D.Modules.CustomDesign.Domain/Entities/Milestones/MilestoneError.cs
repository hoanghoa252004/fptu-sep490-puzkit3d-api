using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

public static class MilestoneError
{
    public static Error InvalidName() => Error.Validation(
        "Milestone.InvalidName",
        "Milestone name cannot be empty.");

    public static Error InvalidSequenceOrder() => Error.Validation(
        "Milestone.InvalidSequenceOrder",
        "Milestone sequence order must be greater than 0.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Milestone.NotFound",
        $"Milestone with ID '{id}' was not found.");
}
