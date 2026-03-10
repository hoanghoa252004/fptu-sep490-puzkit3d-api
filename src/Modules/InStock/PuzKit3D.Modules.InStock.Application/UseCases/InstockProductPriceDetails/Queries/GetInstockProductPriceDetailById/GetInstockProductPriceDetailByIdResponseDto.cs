namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailById;

public sealed record GetInstockProductPriceDetailByIdResponseDto(
    Guid Id,
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
