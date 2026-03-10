using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.UpdateInstockProductVariant;

public sealed record UpdateInstockProductVariantCommand(
    Guid VariantId,
    string? Color = null,
    int? AssembledLengthMm = null,
    int? AssembledWidthMm = null,
    int? AssembledHeightMm = null) : ICommand;

