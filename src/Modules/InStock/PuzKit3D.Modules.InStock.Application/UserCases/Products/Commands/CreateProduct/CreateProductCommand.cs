using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int InitialStock) : ICommandT<Guid>;
