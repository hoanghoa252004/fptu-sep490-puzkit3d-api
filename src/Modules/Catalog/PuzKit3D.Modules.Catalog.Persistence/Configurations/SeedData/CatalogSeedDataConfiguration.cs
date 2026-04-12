using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations.SeedData;

internal static class CatalogSeedDataConfiguration
{
    // Master data IDs - Same as InStock replica tables
    // Topic IDs ===============================================================================================
    private static readonly Guid TopicId1 = Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1");
    private static readonly Guid TopicId2 = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2");
    private static readonly Guid TopicId3 = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3");
    private static readonly Guid TopicId4 = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4");
    private static readonly Guid TopicId5 = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5");

    // ===== Animals children =====
    private static readonly Guid TopicId6 = Guid.Parse("a1a1a1a1-0001-0001-0001-000000000001");
    private static readonly Guid TopicId7 = Guid.Parse("a1a1a1a1-0002-0002-0002-000000000002");

    // ===== Vehicles children =====
    private static readonly Guid TopicId8 = Guid.Parse("b2b2b2b2-0001-0001-0001-000000000001");
    private static readonly Guid TopicId9 = Guid.Parse("b2b2b2b2-0002-0002-0002-000000000002");

    // ===== Architecture children =====
    private static readonly Guid TopicId10 = Guid.Parse("c3c3c3c3-0001-0001-0001-000000000001");
    private static readonly Guid TopicId11 = Guid.Parse("c3c3c3c3-0002-0002-0002-000000000002");

    // ===== Nature children =====
    private static readonly Guid TopicId12 = Guid.Parse("d4d4d4d4-0001-0001-0001-000000000001");
    private static readonly Guid TopicId13 = Guid.Parse("d4d4d4d4-0002-0002-0002-000000000002");

    // ===== Fantasy children =====
    private static readonly Guid TopicId14 = Guid.Parse("e5e5e5e5-0001-0001-0001-000000000001");
    private static readonly Guid TopicId15 = Guid.Parse("e5e5e5e5-0002-0002-0002-000000000002");

    // Material IDs ============================================================================================
    private static readonly Guid MaterialId1 = Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1");
    private static readonly Guid MaterialId2 = Guid.Parse("a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2");
    private static readonly Guid MaterialId3 = Guid.Parse("b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3");
    private static readonly Guid MaterialId4 = Guid.Parse("c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4");
    private static readonly Guid MaterialId5 = Guid.Parse("d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5");

    // Capability IDs ==========================================================================================
    private static readonly Guid CapabilityId1 = Guid.Parse("e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1");
    private static readonly Guid CapabilityId2 = Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2");
    private static readonly Guid CapabilityId3 = Guid.Parse("a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3");
    private static readonly Guid CapabilityId4 = Guid.Parse("b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4");
    private static readonly Guid CapabilityId5 = Guid.Parse("c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5");

    // Assembly Method IDs =====================================================================================
    private static readonly Guid AssemblyMethodId1 = Guid.Parse("d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1");
    private static readonly Guid AssemblyMethodId2 = Guid.Parse("e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2");
    private static readonly Guid AssemblyMethodId3 = Guid.Parse("f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3");
    private static readonly Guid AssemblyMethodId4 = Guid.Parse("a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4");
    private static readonly Guid AssemblyMethodId5 = Guid.Parse("b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5");

    // Drive IDs ===============================================================================================
    private static readonly Guid DriveId1 = Guid.Parse("11111111-1111-1111-1111-111111111111"); // None
    private static readonly Guid DriveId2 = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Motor
    private static readonly Guid DriveId3 = Guid.Parse("33333333-3333-3333-3333-333333333333"); // Gearbox
    private static readonly Guid DriveId4 = Guid.Parse("44444444-4444-4444-4444-444444444444"); // LED
    private static readonly Guid DriveId5 = Guid.Parse("55555555-5555-5555-5555-555555555555"); // Music Box

    // CapabilityMaterialAssembly IDs ==========================================================================
    private static readonly Guid CMAId1 = Guid.Parse("90000000-0000-0000-0000-000000000001");
    private static readonly Guid CMAId2 = Guid.Parse("90000000-0000-0000-0000-000000000002");
    private static readonly Guid CMAId3 = Guid.Parse("90000000-0000-0000-0000-000000000003");
    private static readonly Guid CMAId4 = Guid.Parse("90000000-0000-0000-0000-000000000004");
    private static readonly Guid CMAId5 = Guid.Parse("90000000-0000-0000-0000-000000000005");
    private static readonly Guid CMAId6 = Guid.Parse("90000000-0000-0000-0000-000000000006");
    private static readonly Guid CMAId7 = Guid.Parse("90000000-0000-0000-0000-000000000007");
    private static readonly Guid CMAId8 = Guid.Parse("90000000-0000-0000-0000-000000000008");
    private static readonly Guid CMAId9 = Guid.Parse("90000000-0000-0000-0000-000000000009");
    private static readonly Guid CMAId10 = Guid.Parse("90000000-0000-0000-0000-000000000010");

    // TopicMaterialCapability IDs =============================================================================
    private static readonly Guid TMCId1 = Guid.Parse("80000000-0000-0000-0000-000000000001");
    private static readonly Guid TMCId2 = Guid.Parse("80000000-0000-0000-0000-000000000002");
    private static readonly Guid TMCId3 = Guid.Parse("80000000-0000-0000-0000-000000000003");
    private static readonly Guid TMCId4 = Guid.Parse("80000000-0000-0000-0000-000000000004");
    private static readonly Guid TMCId5 = Guid.Parse("80000000-0000-0000-0000-000000000005");
    private static readonly Guid TMCId6 = Guid.Parse("80000000-0000-0000-0000-000000000006");
    private static readonly Guid TMCId7 = Guid.Parse("80000000-0000-0000-0000-000000000007");
    private static readonly Guid TMCId8 = Guid.Parse("80000000-0000-0000-0000-000000000008");
    private static readonly Guid TMCId9 = Guid.Parse("80000000-0000-0000-0000-000000000009");
    private static readonly Guid TMCId10 = Guid.Parse("80000000-0000-0000-0000-000000000010");

    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //==========================================================================================================
    public static void SeedTopics(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>().HasData(
            new
            {
                Id = TopicId.From(TopicId1),
                Name = "Animals",
                Slug = "animals",
                Description = "Animal themed 3D puzzles",
                ParentId = (TopicId?)null,
                FactorPercentage = 1.1m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId2),
                Name = "Vehicles",
                Slug = "vehicles",
                Description = "Vehicle and transportation themed puzzles",
                ParentId = (TopicId?)null,
                FactorPercentage = 1.3m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId3),
                Name = "Architecture",
                Slug = "architecture",
                Description = "Famous buildings and landmarks",
                ParentId = (TopicId?)null,
                FactorPercentage = 1.5m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId4),
                Name = "Nature",
                Slug = "nature",
                Description = "Natural landscapes and scenery",
                ParentId = (TopicId?)null,
                FactorPercentage = 1.0m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId5),
                Name = "Fantasy",
                Slug = "fantasy",
                Description = "Fantasy creatures and magical worlds",
                ParentId = (TopicId?)null,
                FactorPercentage = 1.6m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            // ===== Animals =====
            new
            {
                Id = TopicId.From(TopicId6),
                Name = "Wild Animals",
                Slug = "wild-animals",
                Description = "Lions, tigers and other wild animals",
                ParentId = TopicId.From(TopicId1),
                FactorPercentage = 1.2m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId7),
                Name = "Pets",
                Slug = "pets",
                Description = "Cats, dogs and domestic animals",
                ParentId = TopicId.From(TopicId1),
                FactorPercentage = 1.0m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },

            // ===== Vehicles =====
            new
            {
                Id = TopicId.From(TopicId8),
                Name = "Cars",
                Slug = "cars",
                Description = "Modern and classic cars",
                ParentId = TopicId.From(TopicId2),
                FactorPercentage = 1.2m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId9),
                Name = "Aircraft",
                Slug = "aircraft",
                Description = "Airplanes and helicopters",
                ParentId = TopicId.From(TopicId2),
                FactorPercentage = 1.4m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },

            // ===== Architecture =====
            new
            {
                Id = TopicId.From(TopicId10),
                Name = "Landmarks",
                Slug = "landmarks",
                Description = "Famous world landmarks",
                ParentId = TopicId.From(TopicId3),
                FactorPercentage = 1.6m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId11),
                Name = "Houses",
                Slug = "houses",
                Description = "Residential buildings and houses",
                ParentId = TopicId.From(TopicId3),
                FactorPercentage = 1.3m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },

            // ===== Nature =====
            new
            {
                Id = TopicId.From(TopicId12),
                Name = "Mountains",
                Slug = "mountains",
                Description = "Mountain landscapes",
                ParentId = TopicId.From(TopicId4),
                FactorPercentage = 1.1m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId13),
                Name = "Oceans",
                Slug = "oceans",
                Description = "Ocean and sea environments",
                ParentId = TopicId.From(TopicId4),
                FactorPercentage = 1.2m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },

            // ===== Fantasy =====
            new
            {
                Id = TopicId.From(TopicId14),
                Name = "Dragons",
                Slug = "dragons",
                Description = "Dragon creatures and models",
                ParentId = TopicId.From(TopicId5),
                FactorPercentage = 1.8m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = TopicId.From(TopicId15),
                Name = "Magic Worlds",
                Slug = "magic-worlds",
                Description = "Fantasy magical environments",
                ParentId = TopicId.From(TopicId5),
                FactorPercentage = 1.7m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    //==========================================================================================================
    public static void SeedMaterials(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>().HasData(
            new
            {
                Id = MaterialId.From(MaterialId1),
                Name = "Wood",
                Slug = "wood",
                Description = "Natural wood material",
                FactorPercentage = 1.0m,
                BasePrice = 4000m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId.From(MaterialId2),
                Name = "Plastic",
                Slug = "plastic",
                Description = "Durable plastic material",
                FactorPercentage = 0.85m,
                BasePrice = 2000m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId.From(MaterialId3),
                Name = "Metal",
                Slug = "metal",
                Description = "Premium metal components",
                FactorPercentage = 1.6m,
                BasePrice = 9000m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId.From(MaterialId4),
                Name = "Cardboard",
                Slug = "cardboard",
                Description = "Eco-friendly cardboard material",
                FactorPercentage = 0.7m,
                BasePrice = 1000m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = MaterialId.From(MaterialId5),
                Name = "Composite",
                Slug = "composite",
                Description = "Advanced composite materials",
                FactorPercentage = 1.3m,
                BasePrice = 7000m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    //==========================================================================================================
    public static void SeedCapabilities(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Capability>().HasData(
            new
            {
                Id = CapabilityId.From(CapabilityId1),
                Name = "Static Display",
                Slug = "static-display",
                Description = "Static model for display only",
                FactorPercentage = 1.0m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId.From(CapabilityId2),
                Name = "Move with Motor",
                Slug = "move-with-motor",
                Description = "Model with electric motor-powered movement",
                FactorPercentage = 1.8m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId.From(CapabilityId3),
                Name = "Manual Movement",
                Slug = "manual-movement",
                Description = "Model operated by manual movement",
                FactorPercentage = 1.3m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId.From(CapabilityId4),
                Name = "LED Light Feature",
                Slug = "led-light-feature",
                Description = "Model with LED lighting effects",
                FactorPercentage = 1.4m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CapabilityId.From(CapabilityId5),
                Name = "Musical Gear",
                Slug = "musical-gear",
                Description = "Model with musical features via rotating mechanism",
                FactorPercentage = 1.6m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    //==========================================================================================================
    public static void SeedAssemblyMethods(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssemblyMethod>().HasData(
            new
            {
                Id = AssemblyMethodId.From(AssemblyMethodId1),
                Name = "Snap-Fit",
                Slug = "snap-fit",
                Description = "Easy snap assembly without tools",
                FactorPercentage = 1.1m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId.From(AssemblyMethodId2),
                Name = "Glue Assembly",
                Slug = "glue-assembly",
                Description = "Assembly using adhesive bonding",
                FactorPercentage = 1.0m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId.From(AssemblyMethodId3),
                Name = "Screw Assembly",
                Slug = "screw-assembly",
                Description = "Assembly using screws and bolts",
                FactorPercentage = 1.3m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId.From(AssemblyMethodId4),
                Name = "Friction Fit",
                Slug = "friction-fit",
                Description = "Assembly using tight fitting pieces",
                FactorPercentage = 1.15m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = AssemblyMethodId.From(AssemblyMethodId5),
                Name = "Magnetic Assembly",
                Slug = "magnetic-assembly",
                Description = "Assembly using magnetic connections",
                FactorPercentage = 1.4m,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    //==========================================================================================================
    public static void SeedDrives(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Drive>().HasData(
            new
            {
                Id = DriveId.From(DriveId1),
                Name = "Router",
                Description = "Router module for rotate",
                MinVolume = 5,
                QuantityInStock = 5,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = DriveId.From(DriveId2),
                Name = "Motor",
                Description = "Electric motor drive",
                MinVolume = 50, // cm³
                QuantityInStock = 100,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = DriveId.From(DriveId3),
                Name = "Gearbox",
                Description = "Mechanical gear system",
                MinVolume = 30,
                QuantityInStock = 150,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = DriveId.From(DriveId4),
                Name = "LED Module",
                Description = "LED lighting system",
                MinVolume = 10,
                QuantityInStock = 200,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = DriveId.From(DriveId5),
                Name = "Music Box",
                Description = "Mechanical music box",
                MinVolume = 50,
                QuantityInStock = 50,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

    //==========================================================================================================
    public static void SeedCapabilityDrives(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CapabilityDrive>().HasData(
            // ===== Static =====
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId1),
                DriveId = DriveId.From(DriveId1) // None
            },
            // ===== Manual =====
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId3),
                DriveId = DriveId.From(DriveId3) // Gearbox
            },
            // ===== Motor (MULTI DRIVE) =====
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId2),
                DriveId = DriveId.From(DriveId2) // Motor
            },
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId2),
                DriveId = DriveId.From(DriveId3) // Gearbox
            },
            // ===== LED =====
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId4),
                DriveId = DriveId.From(DriveId4) // LED Module
            },
            // ===== Musical (MULTI DRIVE) =====
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId5),
                DriveId = DriveId.From(DriveId5) // Music Box
            },
            new
            {
                CapabilityId = CapabilityId.From(CapabilityId5),
                DriveId = DriveId.From(DriveId3) // Gearbox
            }
        );
    }

    //==========================================================================================================
    public static void SeedCapabilityMaterialAssembly(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CapabilityMaterialAssembly>().HasData(
            // ===== STATIC (rộng nhất) =====
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId1), 
                CapabilityId = CapabilityId.From(CapabilityId1), 
                MaterialId = MaterialId.From(MaterialId1), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId1), 
                IsActive = true 
            },
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId2), 
                CapabilityId = CapabilityId.From(CapabilityId1), 
                MaterialId = MaterialId.From(MaterialId4), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId2), 
                IsActive = true 
            },
            // ===== MANUAL =====
            new 
            {
                Id = CapabilityMaterialAssemblyId.From(CMAId3), 
                CapabilityId = CapabilityId.From(CapabilityId3), 
                MaterialId = MaterialId.From(MaterialId1), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId4), 
                IsActive = true 
            },
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId4), 
                CapabilityId = CapabilityId.From(CapabilityId3), 
                MaterialId = MaterialId.From(MaterialId2), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId1), 
                IsActive = true 
            },
            // ===== MOTOR =====
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId5), 
                CapabilityId = CapabilityId.From(CapabilityId2), 
                MaterialId = MaterialId.From(MaterialId1), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId3), 
                IsActive = true 
            },
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId6), 
                CapabilityId = CapabilityId.From(CapabilityId2), 
                MaterialId = MaterialId.From(MaterialId2), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId1), 
                IsActive = true 
            },

            // ===== LED =====
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId7), 
                CapabilityId = CapabilityId.From(CapabilityId4), 
                MaterialId = MaterialId.From(MaterialId2), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId1), 
                IsActive = true 
            },
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId8), 
                CapabilityId = CapabilityId.From(CapabilityId4), 
                MaterialId = MaterialId.From(MaterialId3), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId5), 
                IsActive = true 
            },
            // ===== MUSICAL =====
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId9), 
                CapabilityId = CapabilityId.From(CapabilityId5), 
                MaterialId = MaterialId.From(MaterialId3), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId3), 
                IsActive = true 
            },
            new 
            { 
                Id = CapabilityMaterialAssemblyId.From(CMAId10), 
                CapabilityId = CapabilityId.From(CapabilityId5), 
                MaterialId = MaterialId.From(MaterialId1), 
                AssemblyId = AssemblyMethodId.From(AssemblyMethodId3), 
                IsActive = true 
            }
        );
    }

    //==========================================================================================================
    public static void SeedTopicMaterialCapability(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TopicMaterialCapability>().HasData(
            // ===== Animals =====
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId1), 
                TopicId = TopicId.From(TopicId1), 
                MaterialId = MaterialId.From(MaterialId1), 
                CapabilityId = CapabilityId.From(CapabilityId1), 
                IsActive = true 
            },
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId2), 
                TopicId = TopicId.From(TopicId1), 
                MaterialId = MaterialId.From(MaterialId1), 
                CapabilityId = CapabilityId.From(CapabilityId3), 
                IsActive = true 
            },
            // ===== Vehicles =====
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId3), 
                TopicId = TopicId.From(TopicId2), 
                MaterialId = MaterialId.From(MaterialId2), 
                CapabilityId = CapabilityId.From(CapabilityId2), 
                IsActive = true 
            },
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId4), 
                TopicId = TopicId.From(TopicId2), 
                MaterialId = MaterialId.From(MaterialId3), 
                CapabilityId = CapabilityId.From(CapabilityId2), 
                IsActive = true 
            },
            // ===== Architecture =====
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId5), 
                TopicId = TopicId.From(TopicId3), 
                MaterialId = MaterialId.From(MaterialId1), 
                CapabilityId = CapabilityId.From(CapabilityId1), 
                IsActive = true 
            },
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId6), 
                TopicId = TopicId.From(TopicId3), 
                MaterialId = MaterialId.From(MaterialId3), 
                CapabilityId = CapabilityId.From(CapabilityId4), 
                IsActive = true 
            },
            // ===== Nature =====
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId7), 
                TopicId = TopicId.From(TopicId4), 
                MaterialId = MaterialId.From(MaterialId4), 
                CapabilityId = CapabilityId.From(CapabilityId1), 
                IsActive = true 
            },
            // ===== Fantasy =====
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId8), 
                TopicId = TopicId.From(TopicId5), 
                MaterialId = MaterialId.From(MaterialId3), 
                CapabilityId = CapabilityId.From(CapabilityId2), 
                IsActive = true 
            },
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId9), 
                TopicId = TopicId.From(TopicId5), 
                MaterialId = MaterialId.From(MaterialId3), 
                CapabilityId = CapabilityId.From(CapabilityId4), 
                IsActive = true 
            },
            new 
            { 
                Id = TopicMaterialCapabilityId.From(TMCId10), 
                TopicId = TopicId.From(TopicId5), 
                MaterialId = MaterialId.From(MaterialId5), 
                CapabilityId = CapabilityId.From(CapabilityId5), 
                IsActive = true 
            }
        );
    }

    //==========================================================================================================
    public static void SeedCatalogMasterData(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedTopics();
        modelBuilder.SeedMaterials();
        modelBuilder.SeedCapabilities();
        modelBuilder.SeedAssemblyMethods();
        modelBuilder.SeedDrives();
        modelBuilder.SeedCapabilityDrives();
        modelBuilder.SeedCapabilityMaterialAssembly();
        modelBuilder.SeedTopicMaterialCapability();
    }
}
