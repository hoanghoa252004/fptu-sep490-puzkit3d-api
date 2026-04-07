using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

public static class WorkflowError
{
    public static Error InvalidStartDate() => Error.Validation(
        "Workflow.InvalidStartDate",
        "Workflow start date cannot be empty.");

    public static Error CannotStartWorkflow() => Error.Conflict(
        "Workflow.CannotStartWorkflow",
        "Workflow can only be started from NotStarted status.");

    public static Error CannotCompleteWorkflow() => Error.Conflict(
        "Workflow.CannotCompleteWorkflow",
        "Workflow can only be completed from InProgress or OnHold status.");

    public static Error CannotPutOnHold() => Error.Conflict(
        "Workflow.CannotPutOnHold",
        "Workflow can only be put on hold from InProgress status.");

    public static Error CannotResumeWorkflow() => Error.Conflict(
        "Workflow.CannotResumeWorkflow",
        "Workflow can only be resumed from OnHold status.");

    public static Error CannotFailWorkflow() => Error.Conflict(
        "Workflow.CannotFailWorkflow",
        "Workflow cannot be marked as failed in its current state.");

    public static Error CannotCancelWorkflow() => Error.Conflict(
        "Workflow.CannotCancelWorkflow",
        "Workflow cannot be cancelled in its current state.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Workflow.NotFound",
        $"Workflow with ID '{id}' was not found.");
}
