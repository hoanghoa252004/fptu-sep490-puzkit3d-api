using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations.SeedData;

internal static class InstockSeedDataConfiguration
{
    // Master data IDs
    private static readonly Guid TopicId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid TopicId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid AssemblyMethodId1 = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid AssemblyMethodId2 = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly Guid MaterialId1 = Guid.Parse("55555555-5555-5555-5555-555555555555");
    private static readonly Guid MaterialId2 = Guid.Parse("66666666-6666-6666-6666-666666666666");
    private static readonly Guid CapabilityId1 = Guid.Parse("77777777-7777-7777-7777-777777777777");
    private static readonly Guid CapabilityId2 = Guid.Parse("88888888-8888-8888-8888-888888888888");

    // Price IDs
    private static readonly Guid StandardPriceId = Guid.Parse("99999999-9999-9999-9999-999999999991");
    private static readonly Guid SalePriceId = Guid.Parse("99999999-9999-9999-9999-999999999992");

    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedReplicas(this ModelBuilder modelBuilder)
    {
        // Seed Topics
        modelBuilder.Entity<TopicReplica>().HasData(
            new
            {
                Id = TopicId1,
                Name = "Animals",
                Slug = "animals",
                Description = "Animal themed puzzles",
                ParentId = (Guid?)null,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId2,
                Name = "Vehicles",
                Slug = "vehicles",
                Description = "Vehicle themed puzzles",
                ParentId = (Guid?)null,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );

        // Seed Assembly Methods
        modelBuilder.Entity<AssemblyMethodReplica>().HasData(
            new
            {
                Id = AssemblyMethodId1,
                Name = "Snap-Fit",
                Slug = "snap-fit",
                Description = "Easy snap assembly",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId2,
                Name = "Glue-Based",
                Slug = "glue-based",
                Description = "Requires glue assembly",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );

        // Seed Materials
        modelBuilder.Entity<MaterialReplica>().HasData(
            new
            {
                Id = MaterialId1,
                Name = "Wood",
                Slug = "wood",
                Description = "Natural wood material",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId2,
                Name = "Plastic",
                Slug = "plastic",
                Description = "Durable plastic material",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );

        // Seed Capabilities
        modelBuilder.Entity<CapabilityReplica>().HasData(
            new
            {
                Id = CapabilityId1,
                Name = "Beginner Friendly",
                Slug = "beginner-friendly",
                Description = "Suitable for beginners",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId2,
                Name = "Advanced Building",
                Slug = "advanced-building",
                Description = "For advanced builders",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    public static void SeedPrices(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InstockPrice>().HasData(
            new
            {
                Id = InstockPriceId.From(StandardPriceId),
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
                Id = InstockPriceId.From(SalePriceId),
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

    public static void SeedProducts(this ModelBuilder modelBuilder)
    {
        var products = new[]
        {
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000001")),
                Code = "INP001",
                Slug = "lion-3d-puzzle",
                Name = "Lion 3D Puzzle",
                TotalPieceCount = 150,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 120,
                ThumbnailUrl = "https://example.com/lion.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/lion-preview.jpg\"}",
                Description = "Beautiful lion 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000002")),
                Code = "INP002",
                Slug = "elephant-3d-puzzle",
                Name = "Elephant 3D Puzzle",
                TotalPieceCount = 200,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 180,
                ThumbnailUrl = "https://example.com/elephant.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/elephant-preview.jpg\"}",
                Description = "Majestic elephant 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000003")),
                Code = "INP003",
                Slug = "eagle-3d-puzzle",
                Name = "Eagle 3D Puzzle",
                TotalPieceCount = 180,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 240,
                ThumbnailUrl = "https://example.com/eagle.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/eagle-preview.jpg\"}",
                Description = "Soaring eagle 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId2,
                CapabilityId = CapabilityId2,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000004")),
                Code = "INP004",
                Slug = "sports-car-3d-puzzle",
                Name = "Sports Car 3D Puzzle",
                TotalPieceCount = 250,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 300,
                ThumbnailUrl = "https://example.com/sports-car.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/sports-car-preview.jpg\"}",
                Description = "Sleek sports car 3D puzzle",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId2,
                CapabilityId = CapabilityId2,
                MaterialId = MaterialId2,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000005")),
                Code = "INP005",
                Slug = "airplane-3d-puzzle",
                Name = "Airplane 3D Puzzle",
                TotalPieceCount = 220,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 200,
                ThumbnailUrl = "https://example.com/airplane.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/airplane-preview.jpg\"}",
                Description = "Flying airplane 3D puzzle",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId2,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000006")),
                Code = "INP006",
                Slug = "motorcycle-3d-puzzle",
                Name = "Motorcycle 3D Puzzle",
                TotalPieceCount = 180,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 150,
                ThumbnailUrl = "https://example.com/motorcycle.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/motorcycle-preview.jpg\"}",
                Description = "Cool motorcycle 3D puzzle",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId2,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000007")),
                Code = "INP007",
                Slug = "tiger-3d-puzzle",
                Name = "Tiger 3D Puzzle",
                TotalPieceCount = 170,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 130,
                ThumbnailUrl = "https://example.com/tiger.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/tiger-preview.jpg\"}",
                Description = "Fierce tiger 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000008")),
                Code = "INP008",
                Slug = "dolphin-3d-puzzle",
                Name = "Dolphin 3D Puzzle",
                TotalPieceCount = 130,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 100,
                ThumbnailUrl = "https://example.com/dolphin.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/dolphin-preview.jpg\"}",
                Description = "Playful dolphin 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000009")),
                Code = "INP009",
                Slug = "helicopter-3d-puzzle",
                Name = "Helicopter 3D Puzzle",
                TotalPieceCount = 190,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 170,
                ThumbnailUrl = "https://example.com/helicopter.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/helicopter-preview.jpg\"}",
                Description = "Flying helicopter 3D puzzle",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId1,
                CapabilityId = CapabilityId1,
                MaterialId = MaterialId2,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000010")),
                Code = "INP010",
                Slug = "dragon-3d-puzzle",
                Name = "Dragon 3D Puzzle",
                TotalPieceCount = 300,
                DifficultLevel = "Advanced",
                EstimatedBuildTime = 360,
                ThumbnailUrl = "https://example.com/dragon.jpg",
                PreviewAsset = "{\"main\":\"https://example.com/dragon-preview.jpg\"}",
                Description = "Mythical dragon 3D puzzle",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId2,
                CapabilityId = CapabilityId2,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        };

        modelBuilder.Entity<InstockProduct>().HasData(products);
    }

    public static void SeedVariants(this ModelBuilder modelBuilder)
    {
        var variants = new List<object>();
        var priceDetails = new List<object>();
        var inventories = new List<object>();
        var random = new Random(123); // Fixed seed for reproducibility

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

                // Variant
                variants.Add(new
                {
                    Id = InstockProductVariantId.From(variantId),
                    InstockProductId = InstockProductId.From(productId),
                    Sku = $"SKU{skuCounter:D3}",
                    Color = color,
                    AssembledLengthMm = length,
                    AssembledWidthMm = width,
                    AssembledHeightMm = height,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Standard Price Detail
                var standardPriceDetailId = Guid.NewGuid();
                var standardPrice = random.Next(100, 1001) * 1000m; // 100,000 - 1,000,000
                priceDetails.Add(new
                {
                    Id = InstockProductPriceDetailId.From(standardPriceDetailId),
                    InstockPriceId = InstockPriceId.From(StandardPriceId),
                    InstockProductVariantId = InstockProductVariantId.From(variantId),
                    UnitPrice = standardPrice,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Sale Price Detail
                var salePriceDetailId = Guid.NewGuid();
                var salePrice = standardPrice * 0.8m; // 20% discount
                priceDetails.Add(new
                {
                    Id = InstockProductPriceDetailId.From(salePriceDetailId),
                    InstockPriceId = InstockPriceId.From(SalePriceId),
                    InstockProductVariantId = InstockProductVariantId.From(variantId),
                    UnitPrice = salePrice,
                    IsActive = true,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                // Inventory
                var inventoryQuantity = random.Next(0, 201); // 0-200
                inventories.Add(new
                {
                    Id = InstockInventoryId.From(Guid.NewGuid()),
                    InstockProductVariantId = InstockProductVariantId.From(variantId),
                    TotalQuantity = inventoryQuantity,
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                });

                skuCounter++;
            }
        }

        modelBuilder.Entity<InstockProductVariant>().HasData(variants);
        modelBuilder.Entity<InstockProductPriceDetail>().HasData(priceDetails);
        modelBuilder.Entity<InstockInventory>().HasData(inventories);
    }
}
