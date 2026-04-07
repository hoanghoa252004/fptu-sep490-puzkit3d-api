using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

public static class PhaseError
{
    public static Error InvalidName() => Error.Validation(
        "Phase.InvalidName",
        "Phase name cannot be empty.");

    public static Error InvalidSequenceOrder() => Error.Validation(
        "Phase.InvalidSequenceOrder",
        "Phase sequence order must be greater than 0.");

    public static Error InvalidBasePrice() => Error.Validation(
        "Phase.InvalidBasePrice",
        "Phase base price cannot be negative.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Phase.NotFound",
        $"Phase with ID '{id}' was not found.");
}
