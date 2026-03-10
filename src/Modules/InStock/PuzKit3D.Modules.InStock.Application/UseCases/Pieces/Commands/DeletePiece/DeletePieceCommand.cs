using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.DeletePiece;

public sealed record DeletePieceCommand(Guid ProductId, Guid PartId, Guid PieceId) : ICommand;
