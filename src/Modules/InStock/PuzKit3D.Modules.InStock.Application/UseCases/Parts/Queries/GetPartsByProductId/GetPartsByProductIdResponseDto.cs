namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartsByProductId;

public sealed record GetPartsByProductIdResponseDto(
    Guid Id,
    string Name,
    string PartType,
    string Code,
    int Quantity,
    int TotalPieces);
