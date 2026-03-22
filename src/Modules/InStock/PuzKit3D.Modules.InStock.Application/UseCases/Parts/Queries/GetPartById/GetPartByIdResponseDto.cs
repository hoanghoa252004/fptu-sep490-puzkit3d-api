namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartById;

public sealed record GetPartByIdResponseDto(
    Guid Id,
    string Name,
    string PartType,
    string Code,
    int Quantity);
