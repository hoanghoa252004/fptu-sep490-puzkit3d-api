using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.UpdatePiece;

public sealed record UpdatePieceCommand(
    Guid ProductId,
    Guid PartId,
    Guid PieceId,
    int Quantity,
    Guid? NewPartId) : ICommand;
