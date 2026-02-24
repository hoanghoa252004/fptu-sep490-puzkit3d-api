namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;

public sealed record GetProductByIdResponseDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stock);
