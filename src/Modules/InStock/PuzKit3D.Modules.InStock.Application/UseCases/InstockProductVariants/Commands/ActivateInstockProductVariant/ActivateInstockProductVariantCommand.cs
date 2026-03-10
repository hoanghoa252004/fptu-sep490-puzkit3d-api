using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.ActivateInstockProductVariant;

public sealed record ActivateInstockProductVariantCommand(Guid VariantId) : ICommand;
