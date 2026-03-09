namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPiecesByPartId;

public sealed record GetPiecesByPartIdResponseDto(
    Guid Id,
    string Code,
    int Quantity);
