using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Products;

public sealed class ProductError
{
    public static Error InsufficientStock(int available, int requested) =>
            Error.Failure(
                "Product.InsufficientStock",
                $"Insufficient stock. Available: {available}, Requested: {requested}");

    public static Error InvalidPrice(decimal price) =>
        Error.Failure(
            "Product.InvalidPrice",
            $"Price must be greater than zero. Provided: {price}");

    public static Error InvalidName() =>
        Error.Failure(
            "Product.InvalidName",
            "Product name cannot be empty");

    public static Error InvalidStock(int stock) =>
        Error.Failure(
            "Product.InvalidStock",
            $"Stock cannot be negative. Provided: {stock}");

    public static Error NotFound(Guid productId) =>
        Error.NotFound(
            "Product.NotFound",
            $"Product with Id [{productId}] was not found");

    public static Error DuplicateName(string name) =>
        Error.Conflict(
            "Product.DuplicateName",
            $"Product with name '{name}' already exists");
}
