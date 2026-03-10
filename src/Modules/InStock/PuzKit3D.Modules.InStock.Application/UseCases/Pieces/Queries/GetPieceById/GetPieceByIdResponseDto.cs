namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPieceById;

public sealed record GetPieceByIdResponseDto(
    Guid Id,
    string Code,
    int Quantity,
    Guid PartId);
