using PuzKit3D.Contract.Abstractions.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Domain.Entities.Products.Errors;

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
}
