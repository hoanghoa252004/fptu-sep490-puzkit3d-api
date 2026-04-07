using PuzKit3D.SharedKernel.Domain.Errors;
using System;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

public static class ProposalError
{
    public static Error InvalidCode() => Error.Validation(
        "Proposal.InvalidCode",
        "Proposal code cannot be empty.");

    public static Error InvalidLaborCost() => Error.Validation(
        "Proposal.InvalidLaborCost",
        "Proposal labor cost cannot be negative.");

    public static Error InvalidProductCost() => Error.Validation(
        "Proposal.InvalidProductCost",
        "Proposal product cost cannot be negative.");

    public static Error InvalidTotalWeightPercent() => Error.Validation(
        "Proposal.InvalidTotalWeightPercent",
        "Proposal total weight percent must be between 0 and 100.");

    public static Error InvalidTotalAmount() => Error.Validation(
        "Proposal.InvalidTotalAmount",
        "Proposal total amount cannot be negative.");

    public static Error CannotApproveProposal() => Error.Conflict(
        "Proposal.CannotApproveProposal",
        "Proposal cannot be approved in its current state.");

    public static Error CannotRejectProposal() => Error.Conflict(
        "Proposal.CannotRejectProposal",
        "Proposal cannot be rejected in its current state.");

    public static Error CannotSendProposal() => Error.Conflict(
        "Proposal.CannotSendProposal",
        "Proposal cannot be sent in its current state.");

    public static Error CannotUpdateProposal() => Error.Conflict(
        "Proposal.CannotUpdateProposal",
        "Proposal can only be updated when in Draft status.");

    public static Error InvalidStatusTransition() => Error.Conflict(
        "Proposal.InvalidStatusTransition",
        "The status transition is not allowed.");

    public static Error NotFound(Guid id) => Error.NotFound(
        "Proposal.NotFound",
        $"Proposal with ID '{id}' was not found.");

    public static Error DuplicateCode(string code) => Error.Conflict(
        "Proposal.DuplicateCode",
        $"Proposal with code '{code}' already exists.");
}
