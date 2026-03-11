namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockInventories.Queries.GetInstockInventoryByVariantId;

public sealed record GetInstockInventoryByVariantIdResponseDto(
    Guid Id,
    Guid InstockProductVariantId,
    int TotalQuantity,
    DateTime CreatedAt,
    DateTime UpdatedAt);
