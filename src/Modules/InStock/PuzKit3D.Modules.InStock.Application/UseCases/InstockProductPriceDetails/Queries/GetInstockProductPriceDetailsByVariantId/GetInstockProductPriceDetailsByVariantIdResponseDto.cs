namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailsByVariantId;

public sealed record PriceDetailDto(
    Guid Id,
    Guid PriceId,
    string PriceName,
    int Priority,
    decimal UnitPrice,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record AnonymousPriceDetailDto(
    Guid Id,
    Guid PriceId,
    string PriceName,
    decimal UnitPrice);
