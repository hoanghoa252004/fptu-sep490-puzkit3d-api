using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.UpdateRequest;

public sealed record UpdateCustomDesignRequestCommand(
    Guid Id,
    decimal? DesiredLengthMm,
    decimal? DesiredWidthMm,
    decimal? DesiredHeightMm,
    string? Sketches,
    string? CustomerPrompt,
    DateTime? DesiredDeliveryDate,
    int? DesiredQuantity,
    decimal? TargetBudget,
    string? Status,
    string? Note) : ICommand;



