using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.DeletePart;

public sealed record DeletePartCommand(Guid ProductId, Guid PartId) : ICommand;
