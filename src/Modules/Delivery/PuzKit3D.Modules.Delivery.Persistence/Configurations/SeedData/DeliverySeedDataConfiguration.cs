using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations.SeedData;

internal static class DeliverySeedDataConfiguration
{
    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedPartReplicas(this ModelBuilder modelBuilder)
    {
        var parts = new List<dynamic>();
        var partTypes = new[] { "Structural", "Mechanical", "Decorative" };

        int partCounter = 1;

        // Create 10 products with 10 parts each - Same as InStock module
        for (int productIndex = 1; productIndex <= 10; productIndex++)
        {
            var productId = Guid.Parse($"10000000-0000-0000-0000-{productIndex:D12}");

            for (int partIndex = 0; partIndex < 10; partIndex++)
            {
                var partType = partTypes[partIndex % 3];
                var partId = Guid.Parse($"50000000-{productIndex:D4}-{partIndex:D4}-0000-000000000000");
                var partCode = $"PAR{partCounter:D4}";

                parts.Add(new
                {
                    Id = partId,
                    Name = $"{GetProductName(productIndex)} Part {partIndex + 1}",
                    PartType = partType,
                    Code = partCode,
                    Quantity = 10 + (partIndex * 5), // Varying quantities
                    InstockProductId = productId,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                partCounter++;
            }
        }

        modelBuilder.Entity<PartReplica>().HasData(parts);
    }

    private static string GetProductName(int productIndex) => productIndex switch
    {
        1 => "Lion",
        2 => "Elephant",
        3 => "Eagle",
        4 => "Sports Car",
        5 => "Airplane",
        6 => "Motorcycle",
        7 => "Tiger",
        8 => "Dolphin",
        9 => "Helicopter",
        10 => "Dragon",
        _ => "Product"
    };
}
