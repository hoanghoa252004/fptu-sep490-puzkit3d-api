using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations.SeedData;

internal static class InstockSeedDataConfiguration
{
    // Master data IDs - Same as Catalog
    // Topic IDs
    private static readonly Guid TopicId1 = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
    private static readonly Guid TopicId2 = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2");
    private static readonly Guid TopicId3 = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3");
    private static readonly Guid TopicId4 = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4");
    private static readonly Guid TopicId5 = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5");

    // Material IDs
    private static readonly Guid MaterialId1 = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
    private static readonly Guid MaterialId2 = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2");
    private static readonly Guid MaterialId3 = Guid.Parse("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3");
    private static readonly Guid MaterialId4 = Guid.Parse("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4");
    private static readonly Guid MaterialId5 = Guid.Parse("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5");

    // Capability IDs
    private static readonly Guid CapabilityId1 = Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1");
    private static readonly Guid CapabilityId2 = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2");
    private static readonly Guid CapabilityId3 = Guid.Parse("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3");
    private static readonly Guid CapabilityId4 = Guid.Parse("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4");
    private static readonly Guid CapabilityId5 = Guid.Parse("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5");

    // Assembly Method IDs
    private static readonly Guid AssemblyMethodId1 = Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1");
    private static readonly Guid AssemblyMethodId2 = Guid.Parse("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2");
    private static readonly Guid AssemblyMethodId3 = Guid.Parse("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3");
    private static readonly Guid AssemblyMethodId4 = Guid.Parse("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4");
    private static readonly Guid AssemblyMethodId5 = Guid.Parse("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5");

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
                Description = "Animal themed 3D puzzles",
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
                Description = "Vehicle and transportation themed puzzles",
                ParentId = (Guid?)null,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId3,
                Name = "Architecture",
                Slug = "architecture",
                Description = "Famous buildings and landmarks",
                ParentId = (Guid?)null,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId4,
                Name = "Nature",
                Slug = "nature",
                Description = "Natural landscapes and scenery",
                ParentId = (Guid?)null,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId5,
                Name = "Fantasy",
                Slug = "fantasy",
                Description = "Fantasy creatures and magical worlds",
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
                Description = "Easy snap assembly without tools",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId2,
                Name = "Glue Assembly",
                Slug = "glue-assembly",
                Description = "Assembly using adhesive bonding",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId3,
                Name = "Screw Assembly",
                Slug = "screw-assembly",
                Description = "Assembly using screws and bolts",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId4,
                Name = "Friction Fit",
                Slug = "friction-fit",
                Description = "Assembly using tight fitting pieces",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId5,
                Name = "Magnetic Assembly",
                Slug = "magnetic-assembly",
                Description = "Assembly using magnetic connections",
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
            },
            new
            {
                Id = MaterialId3,
                Name = "Metal",
                Slug = "metal",
                Description = "Premium metal components",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId4,
                Name = "Cardboard",
                Slug = "cardboard",
                Description = "Eco-friendly cardboard material",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId5,
                Name = "Composite",
                Slug = "composite",
                Description = "Advanced composite materials",
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
                Name = "Static Display",
                Slug = "static-display",
                Description = "Static model for display only",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId2,
                Name = "Move with Motor",
                Slug = "move-with-motor",
                Description = "Model with electric motor-powered movement",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId3,
                Name = "Manual Movement",
                Slug = "manual-movement",
                Description = "Model operated by manual movement",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId4,
                Name = "LED Light Feature",
                Slug = "led-light-feature",
                Description = "Model with LED lighting effects",
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId5,
                Name = "Musical Gear",
                Slug = "musical-gear",
                Description = "Model with musical features via rotating mechanism",
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
                Slug = "ugt-24-endurance-racer",
                Name = "UGT-24 Endurance Racer",
                TotalPieceCount = 150,
                DifficultLevel = "Basic",
                EstimatedBuildTime = 120,
                ThumbnailUrl = "instock-products/ugt-24-endurance-racer/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}",
                Description = "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId1,
                MaterialId = MaterialId1,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000002")),
                Code = "INP002",
                Slug = "mad-hornet-airplane",
                Name = "Mad Hornet Airplane",
                TotalPieceCount = 200,
                DifficultLevel = "Intermediate",
                EstimatedBuildTime = 180,
                ThumbnailUrl = "instock-products/mad-hornet-airplane/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}",
                Description = "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId2,
                MaterialId = MaterialId2,
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
                ThumbnailUrl = "instock-products/ugt-24-endurance-racer/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}",
                Description = "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId3,
                MaterialId = MaterialId3,
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
                ThumbnailUrl = "instock-products/mad-hornet-airplane/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}",
                Description = "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId4,
                MaterialId = MaterialId4,
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
                ThumbnailUrl = "instock-products/ugt-24-endurance-racer/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}",
                Description = "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId5,
                MaterialId = MaterialId5,
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
                ThumbnailUrl = "instock-products/ugt-24-endurance-racer/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}",
                Description = "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!",
                TopicId = TopicId3,
                AssemblyMethodId = AssemblyMethodId1,
                MaterialId = MaterialId1,
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
                ThumbnailUrl = "instock-products/mad-hornet-airplane/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}",
                Description = "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!",
                TopicId = TopicId1,
                AssemblyMethodId = AssemblyMethodId2,
                MaterialId = MaterialId2,
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
                ThumbnailUrl = "instock-products/mad-hornet-airplane/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}",
                Description = "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!",
                TopicId = TopicId4,
                AssemblyMethodId = AssemblyMethodId3,
                MaterialId = MaterialId3,
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
                ThumbnailUrl = "instock-products/ugt-24-endurance-racer/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/ugt-24-endurance-racer/image-01.png\",\"additionalProp2\":\"instock-products/ugt-24-endurance-racer/image-02.png\"}",
                Description = "New wooden mechanical 3D puzzle UGT-24 Endurance Racer by Ugears. A large car featuring a blue racing stripe and plastic windows drives 5-6 m thanks to a spring motor. Cool gift!",
                TopicId = TopicId2,
                AssemblyMethodId = AssemblyMethodId4,
                MaterialId = MaterialId4,
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
                ThumbnailUrl = "instock-products/mad-hornet-airplane/thumbnail.png",
                PreviewAsset = "{\"additionalProp1\":\"instock-products/mad-hornet-airplane/image-01.png\",\"additionalProp2\":\"instock-products/mad-hornet-airplane/image-02.png\"}",
                Description = "Wooden 3D puzzle Mad Hornet Airplane from Ugears. Pre-flight check mode and taxi mode. Moves without batteries. Assemble without glue. The perfect gift!",
                TopicId = TopicId5,
                AssemblyMethodId = AssemblyMethodId5,
                MaterialId = MaterialId5,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        };

        modelBuilder.Entity<InstockProduct>().HasData(products);
    }

    public static void SeedVariants(this ModelBuilder modelBuilder)
    {
        // Hardcoded Variant IDs
        var variantIds = new[]
        {
            Guid.Parse("20000000-0001-0000-0000-000000000001"), // Lion Red
            Guid.Parse("20000000-0001-0000-0000-000000000002"), // Lion Blue
            Guid.Parse("20000000-0002-0000-0000-000000000001"), // Elephant Green
            Guid.Parse("20000000-0003-0000-0000-000000000001"), // Eagle Yellow
            Guid.Parse("20000000-0003-0000-0000-000000000002"), // Eagle Black
            Guid.Parse("20000000-0004-0000-0000-000000000001"), // Sports Car White
            Guid.Parse("20000000-0004-0000-0000-000000000002"), // Sports Car Orange
            Guid.Parse("20000000-0005-0000-0000-000000000001"), // Airplane Purple
            Guid.Parse("20000000-0006-0000-0000-000000000001"), // Motorcycle Red
            Guid.Parse("20000000-0006-0000-0000-000000000002"), // Motorcycle Blue
            Guid.Parse("20000000-0007-0000-0000-000000000001"), // Tiger Green
            Guid.Parse("20000000-0008-0000-0000-000000000001"), // Dolphin Yellow
            Guid.Parse("20000000-0008-0000-0000-000000000002"), // Dolphin Black
            Guid.Parse("20000000-0009-0000-0000-000000000001"), // Helicopter White
            Guid.Parse("20000000-0010-0000-0000-000000000001"), // Dragon Orange
            Guid.Parse("20000000-0010-0000-0000-000000000002")  // Dragon Purple
        };

        var variantData = new[]
        {
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000001"), Color: "Red", Length: 218, Width: 159, Height: 261),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000001"), Color: "Blue", Length: 174, Width: 138, Height: 297),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000002"), Color: "Green", Length: 229, Width: 263, Height: 175),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000003"), Color: "Yellow", Length: 131, Width: 171, Height: 224),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000003"), Color: "Black", Length: 182, Width: 292, Height: 289),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000004"), Color: "White", Length: 156, Width: 240, Height: 158),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000004"), Color: "Orange", Length: 225, Width: 187, Height: 127),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000005"), Color: "Purple", Length: 219, Width: 201, Height: 222),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000006"), Color: "Red", Length: 154, Width: 278, Height: 227),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000006"), Color: "Blue", Length: 288, Width: 296, Height: 152),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000007"), Color: "Green", Length: 221, Width: 207, Height: 177),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000008"), Color: "Yellow", Length: 137, Width: 135, Height: 297),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000008"), Color: "Black", Length: 195, Width: 221, Height: 274),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000009"), Color: "White", Length: 146, Width: 111, Height: 102),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000010"), Color: "Orange", Length: 265, Width: 262, Height: 154),
            (ProductId: Guid.Parse("10000000-0000-0000-0000-000000000010"), Color: "Purple", Length: 150, Width: 186, Height: 251)
        };

        var prices = new[] { 745000m, 419000m, 268000m, 869000m, 648000m, 356000m, 909000m, 631000m, 531000m, 615000m, 704000m, 540000m, 330000m, 42000m, 867000m, 509000m };
        var quantities = new[] { 75, 83, 162, 152, 123, 167, 52, 99, 13, 186, 78, 143, 35, 117, 10, 185 };

        var variants = new List<object>();
        var priceDetails = new List<object>();
        var inventories = new List<object>();

        for (int i = 0; i < variantIds.Length; i++)
        {
            var variantId = variantIds[i];
            var data = variantData[i];
            var standardPrice = prices[i];
            var salePrice = standardPrice * 0.8m;
            var quantity = quantities[i];

            // Variant
            variants.Add(new
            {
                Id = InstockProductVariantId.From(variantId),
                InstockProductId = InstockProductId.From(data.ProductId),
                Sku = $"SKU{(i + 1):D3}",
                Color = data.Color,
                AssembledLengthMm = data.Length,
                AssembledWidthMm = data.Width,
                AssembledHeightMm = data.Height,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            });

            // Standard Price Detail
            var standardPriceDetailId = Guid.Parse($"30000000-{(i + 1):D4}-1000-0000-000000000000");
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
            var salePriceDetailId = Guid.Parse($"30000000-{(i + 1):D4}-2000-0000-000000000000");
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
            var inventoryId = Guid.Parse($"40000000-{(i + 1):D4}-0000-0000-000000000000");
            inventories.Add(new
            {
                Id = InstockInventoryId.From(inventoryId),
                InstockProductVariantId = InstockProductVariantId.From(variantId),
                TotalQuantity = quantity,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            });
        }

        modelBuilder.Entity<InstockProductVariant>().HasData(variants);
        modelBuilder.Entity<InstockProductPriceDetail>().HasData(priceDetails);
        modelBuilder.Entity<InstockInventory>().HasData(inventories);
    }

    public static void SeedProductCapabilityDetails(this ModelBuilder modelBuilder)
    {
        var details = new[]
        {
            // Lion: Static Display, Move with Motor
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000001")), CapabilityId = CapabilityId1 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000001")), CapabilityId = CapabilityId2 },
            
            // Elephant: Move with Motor, Manual Movement
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000002")), CapabilityId = CapabilityId2 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000002")), CapabilityId = CapabilityId3 },
            
            // Eagle: Manual Movement, LED Light Feature
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000003")), CapabilityId = CapabilityId3 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000003")), CapabilityId = CapabilityId4 },
            
            // Sports Car: LED Light Feature, Musical Gear
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000004")), CapabilityId = CapabilityId4 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000004")), CapabilityId = CapabilityId5 },
            
            // Airplane: Musical Gear, Static Display
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000005")), CapabilityId = CapabilityId5 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000005")), CapabilityId = CapabilityId1 },
            
            // Motorcycle: Static Display, Move with Motor
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000006")), CapabilityId = CapabilityId1 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000006")), CapabilityId = CapabilityId2 },
            
            // Tiger: Move with Motor, Manual Movement
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000007")), CapabilityId = CapabilityId2 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000007")), CapabilityId = CapabilityId3 },
            
            // Dolphin: Manual Movement, LED Light Feature
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000008")), CapabilityId = CapabilityId3 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000008")), CapabilityId = CapabilityId4 },
            
            // Helicopter: LED Light Feature, Musical Gear
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000009")), CapabilityId = CapabilityId4 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000009")), CapabilityId = CapabilityId5 },
            
            // Dragon: Musical Gear, Static Display
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000010")), CapabilityId = CapabilityId5 },
            new { InstockProductId = InstockProductId.From(Guid.Parse("10000000-0000-0000-0000-000000000010")), CapabilityId = CapabilityId1 }
        };

        modelBuilder.Entity<InstockProductCapabilityDetail>().HasData(details);
    }
}

