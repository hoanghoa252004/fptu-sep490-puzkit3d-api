using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations.SeedData;

internal static class CustomDesignSeedDataConfiguration
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

    // CustomDesignRequirement IDs
    private static readonly Guid RequirementId1 = Guid.Parse("c1c1c1c1-c1c1-c1c1-c1c1-c1c1c1c1c1c1");
    private static readonly Guid RequirementId2 = Guid.Parse("d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2");
    private static readonly Guid RequirementId3 = Guid.Parse("e3e3e3e3-e3e3-e3e3-e3e3-e3e3e3e3e3e3");
    private static readonly Guid RequirementId4 = Guid.Parse("f4f4f4f4-f4f4-f4f4-f4f4-f4f4f4f4f4f4");
    private static readonly Guid RequirementId5 = Guid.Parse("a5a5a5a5-a5a5-a5a5-a5a5-a5a5a5a5a5a5");

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

    public static void SeedCustomDesignRequirements(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomDesignRequirement>().HasData(
            new
            {
                Id = CustomDesignRequirementId.From(RequirementId1),
                Code = "CDR-001",
                TopicId = TopicId1,
                MaterialId = MaterialId1,
                AssemblyMethodId = AssemblyMethodId1,
                Difficulty = DifficultyLevel.Basic,
                MinPartQuantity = 10,
                MaxPartQuantity = 50,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CustomDesignRequirementId.From(RequirementId2),
                Code = "CDR-002",
                TopicId = TopicId2,
                MaterialId = MaterialId2,
                AssemblyMethodId = AssemblyMethodId2,
                Difficulty = DifficultyLevel.Intermediate,
                MinPartQuantity = 20,
                MaxPartQuantity = 100,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CustomDesignRequirementId.From(RequirementId3),
                Code = "CDR-003",
                TopicId = TopicId3,
                MaterialId = MaterialId3,
                AssemblyMethodId = AssemblyMethodId3,
                Difficulty = DifficultyLevel.Advanced,
                MinPartQuantity = 50,
                MaxPartQuantity = 200,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CustomDesignRequirementId.From(RequirementId4),
                Code = "CDR-004",
                TopicId = TopicId4,
                MaterialId = MaterialId4,
                AssemblyMethodId = AssemblyMethodId4,
                Difficulty = DifficultyLevel.Basic,
                MinPartQuantity = 15,
                MaxPartQuantity = 60,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = CustomDesignRequirementId.From(RequirementId5),
                Code = "CDR-005",
                TopicId = TopicId5,
                MaterialId = MaterialId5,
                AssemblyMethodId = AssemblyMethodId5,
                Difficulty = DifficultyLevel.Intermediate,
                MinPartQuantity = 30,
                MaxPartQuantity = 150,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            }
        );
    }

}
