using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.DeleteInstockProductVariant;

public sealed record DeleteInstockProductVariantCommand(Guid VariantId) : ICommand;
