using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.CreatePiece;

public sealed record CreatePieceCommand(
    Guid ProductId,
    Guid PartId,
    int Quantity) : ICommandT<Guid>;

