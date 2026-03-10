namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Queries.GetAllInstockPrices;

public sealed record GetAllInstockPricesResponseDto(
    Guid Id,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record AnonymousInstockPriceDto(
    Guid Id,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority);
