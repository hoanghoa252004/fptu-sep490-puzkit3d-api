using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.CreateRequest;

public sealed record CreateCustomDesignRequestCommand(
    Guid CustomDesignRequirementId,
    decimal DesiredLengthMm,
    decimal DesiredWidthMm,
    decimal DesiredHeightMm,
    string? Sketches,
    string? CustomerPrompt,
    DateTime DesiredDeliveryDate,
    int DesiredQuantity,
    decimal TargetBudget,
    string Type) : ICommandT<Guid>;



