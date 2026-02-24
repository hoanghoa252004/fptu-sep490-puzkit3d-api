using PuzKit3D.Modules.InStock.Domain.Events.Products;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Domain.Entities.Products;

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    private Product(ProductId id, string name, decimal price, int stock) : base(id)
    {
        Name = name;
        Price = price;
        Stock = stock;
    }

    private Product() : base()
    {
    }

    public static ResultT<Product> Create(string name, decimal price, int initialStock)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Product>(ProductError.InvalidName());

        if (price <= 0)
            return Result.Failure<Product>(ProductError.InvalidPrice(price));

        if (initialStock < 0)
            return Result.Failure<Product>(ProductError.InvalidStock(initialStock));

        var productId = ProductId.Create();
        var product = new Product(productId, name, price, initialStock);

        product.RaiseDomainEvent(new ProductCreatedDomainEvent(
            productId.Value,
            name,
            price,
            initialStock));

        return Result.Success(product);
    }

    public Result UpdateStock(int quantity)
    {
        var oldStock = Stock;

        if (Stock + quantity < 0)
            return Result.Failure(ProductError.InsufficientStock(Stock, Math.Abs(quantity)));

        Stock += quantity;

        RaiseDomainEvent(new ProductStockChangedDomainEvent(
            Id.Value,
            oldStock,
            Stock,
            quantity));

        return Result.Success();
    }

    public Result SetPrice(decimal price)
    {
        if (price <= 0)
            return Result.Failure(ProductError.InvalidPrice(price));

        Price = price;
        return Result.Success();
    }
}
