using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations.SeedData;

internal static class CartSeedDataConfiguration
{
    // Price IDs (same as InStock module)
    private static readonly Guid StandardPriceId = Guid.Parse("99999999-9999-9999-9999-999999999991");
    private static readonly Guid SalePriceId = Guid.Parse("99999999-9999-9999-9999-999999999992");

    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedInStockPriceReplicas(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InStockPriceReplica>().HasData(
            new
            {
                Id = StandardPriceId,
                Name = "Standard",
                EffectiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EffectiveTo = new DateTime(2099, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                Priority = 1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = SalePriceId,
                Name = "Sale Summer Vacation",
                EffectiveFrom = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EffectiveTo = new DateTime(2026, 7, 1, 23, 59, 59, DateTimeKind.Utc),
                Priority = 2,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    public static void SeedInStockProductReplicas(this ModelBuilder modelBuilder)
    {
        var products = new[]
        {
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Code = "INP001",
                Slug = "lion-3d-puzzle",
                Name = "Lion 3D Puzzle",
                TotalPieceCount = 150,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 120,
                ThumbnailUrl = "https://example.com/lion.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/lion-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Code = "INP002",
                Slug = "elephant-3d-puzzle",
                Name = "Elephant 3D Puzzle",
                TotalPieceCount = 200,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 180,
                ThumbnailUrl = "https://example.com/elephant.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/elephant-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Code = "INP003",
                Slug = "eagle-3d-puzzle",
                Name = "Eagle 3D Puzzle",
                TotalPieceCount = 180,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 240,
                ThumbnailUrl = "https://example.com/eagle.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/eagle-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CapabilityId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Code = "INP004",
                Slug = "sports-car-3d-puzzle",
                Name = "Sports Car 3D Puzzle",
                TotalPieceCount = 250,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 300,
                ThumbnailUrl = "https://example.com/sports-car.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/sports-car-preview.jpg\"}",
                TopicId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AssemblyMethodId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CapabilityId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                MaterialId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Code = "INP005",
                Slug = "airplane-3d-puzzle",
                Name = "Airplane 3D Puzzle",
                TotalPieceCount = 220,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 200,
                ThumbnailUrl = "https://example.com/airplane.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/airplane-preview.jpg\"}",
                TopicId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                Code = "INP006",
                Slug = "motorcycle-3d-puzzle",
                Name = "Motorcycle 3D Puzzle",
                TotalPieceCount = 180,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 150,
                ThumbnailUrl = "https://example.com/motorcycle.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/motorcycle-preview.jpg\"}",
                TopicId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                Code = "INP007",
                Slug = "tiger-3d-puzzle",
                Name = "Tiger 3D Puzzle",
                TotalPieceCount = 170,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 130,
                ThumbnailUrl = "https://example.com/tiger.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/tiger-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                Code = "INP008",
                Slug = "dolphin-3d-puzzle",
                Name = "Dolphin 3D Puzzle",
                TotalPieceCount = 130,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 100,
                ThumbnailUrl = "https://example.com/dolphin.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/dolphin-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000009"),
                Code = "INP009",
                Slug = "helicopter-3d-puzzle",
                Name = "Helicopter 3D Puzzle",
                TotalPieceCount = 190,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 170,
                ThumbnailUrl = "https://example.com/helicopter.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/helicopter-preview.jpg\"}",
                TopicId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AssemblyMethodId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                CapabilityId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                MaterialId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000010"),
                Code = "INP010",
                Slug = "dragon-3d-puzzle",
                Name = "Dragon 3D Puzzle",
                TotalPieceCount = 300,
                BriefDescription = (string?)null,
                DetailDescription = (string?)null,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 360,
                ThumbnailUrl = "https://example.com/dragon.jpg",
                Specification = (string?)null,
                PreviewAsset = "{\"main\":\"https://example.com/dragon-preview.jpg\"}",
                TopicId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                AssemblyMethodId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                CapabilityId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                MaterialId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        };

        modelBuilder.Entity<InStockProductReplica>().HasData(products);
    }

    public static void SeedInStockVariantsAndRelatedReplicas(this ModelBuilder modelBuilder)
    {
        var variants = new List<object>();
        var priceDetails = new List<object>();
        var inventories = new List<object>();
        var random = new Random(123); // Same seed as InStock module

        var productVariantData = new[]
        {
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000001"), VariantCount: 2),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000002"), VariantCount: 1),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000003"), VariantCount: 2),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000004"), VariantCount: 2),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000005"), VariantCount: 1),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000006"), VariantCount: 2),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000007"), VariantCount: 1),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000008"), VariantCount: 2),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000009"), VariantCount: 1),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000010"), VariantCount: 2)
        };

        var colors = new[] { "Red", "Blue", "Green", "Yellow", "Black", "White", "Orange", "Purple" };
        int skuCounter = 1;
        int colorIndex = 0;

        foreach (var (productId, variantCount) in productVariantData)
        {
            for (int i = 0; i < variantCount; i++)
            {
                var variantId = Guid.NewGuid();
                var color = colors[colorIndex++ % colors.Length];
                var length = random.Next(100, 300);
                var width = random.Next(100, 300);
                var height = random.Next(100, 300);

                // Variant Replica
                variants.Add(new
                {
                    Id = variantId,
                    InStockProductId = productId,
                    Sku = $"SKU{skuCounter:D3}",
                    Color = color,
                    AssembledLengthMm = length,
                    AssembledWidthMm = width,
                    AssembledHeightMm = height,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Standard Price Detail Replica
                var standardPriceDetailId = Guid.NewGuid();
                var standardPrice = random.Next(100, 1001) * 1000m;
                priceDetails.Add(new
                {
                    Id = standardPriceDetailId,
                    InStockPriceId = StandardPriceId,
                    InStockProductVariantId = variantId,
                    UnitPrice = standardPrice,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Sale Price Detail Replica
                var salePriceDetailId = Guid.NewGuid();
                var salePrice = standardPrice * 0.8m;
                priceDetails.Add(new
                {
                    Id = salePriceDetailId,
                    InStockPriceId = SalePriceId,
                    InStockProductVariantId = variantId,
                    UnitPrice = salePrice,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Inventory Replica
                var inventoryQuantity = random.Next(0, 201);
                inventories.Add(new
                {
                    Id = Guid.NewGuid(),
                    InStockProductVariantId = variantId,
                    TotalQuantity = inventoryQuantity,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                skuCounter++;
            }
        }

        modelBuilder.Entity<InStockProductVariantReplica>().HasData(variants);
        modelBuilder.Entity<InStockProductPriceDetailReplica>().HasData(priceDetails);
        modelBuilder.Entity<InStockInventoryReplica>().HasData(inventories);
    }
}
