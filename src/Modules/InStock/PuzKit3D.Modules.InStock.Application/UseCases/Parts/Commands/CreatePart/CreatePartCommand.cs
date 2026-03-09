using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.CreatePart;

public sealed record CreatePartCommand(
    Guid ProductId,
    string Name,
    string PartType) : ICommandT<Guid>;

