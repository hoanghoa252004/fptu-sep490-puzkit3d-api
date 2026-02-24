namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Commands.CreateProduct;

public sealed record CreateProductResponseDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stock);
