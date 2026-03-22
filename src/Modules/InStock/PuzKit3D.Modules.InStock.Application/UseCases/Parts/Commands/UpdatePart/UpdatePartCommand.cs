using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(
    Guid ProductId,
    Guid PartId,
    string? Name,
    PartType? PartType,
    int? Quantity) : ICommand;

