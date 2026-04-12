using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.CreateInstockProductVariant;

public sealed record CreateInstockProductVariantCommand(
Guid ProductId,
string Color,
int AssembledLengthMm,
int AssembledWidthMm,
int AssembledHeightMm,
string PreviewImages,
bool IsActive = false) : ICommandT<Guid>;


