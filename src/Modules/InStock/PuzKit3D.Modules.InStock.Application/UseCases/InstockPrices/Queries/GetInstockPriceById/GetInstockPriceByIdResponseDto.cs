namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetInstockPriceById;

public sealed record GetInstockPriceByIdResponseDto(
    Guid Id,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
