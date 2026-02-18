using PuzKit3D.Contract.Abstractions.Shared.Results;
using PuzKit3D.Domain.Abstractions.Entities;
using PuzKit3D.Domain.Entities.Products.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Domain.Entities.Products;

public class Product : BaseEntity<Guid>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public Result UpdateStock(int quantity)
    {
        if (Stock + quantity < 0)
            return Result.Failure(ProductError.InsufficientStock(Stock, Math.Abs(quantity)));

        Stock += quantity;
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
