using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations.SeedData;

/// <summary>
/// Seed data SQL statements for Catalog module master data
/// Topic, Material, Capability, and AssemblyMethod tables
/// </summary>
internal static class CatalogSeedDataConfiguration
{
    // Topic IDs
    public const string TopicId1 = "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1";
    public const string TopicId2 = "b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2";
    public const string TopicId3 = "c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3";
    public const string TopicId4 = "d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4";
    public const string TopicId5 = "e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5";

    // Material IDs
    public const string MaterialId1 = "f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1";
    public const string MaterialId2 = "a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2";
    public const string MaterialId3 = "b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3";
    public const string MaterialId4 = "c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4";
    public const string MaterialId5 = "d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5";

    // Capability IDs
    public const string CapabilityId1 = "e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1";
    public const string CapabilityId2 = "f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2";
    public const string CapabilityId3 = "a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3";
    public const string CapabilityId4 = "b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4";
    public const string CapabilityId5 = "c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5";

    // Assembly Method IDs
    public const string AssemblyMethodId1 = "d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1";
    public const string AssemblyMethodId2 = "e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2";
    public const string AssemblyMethodId3 = "f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3";
    public const string AssemblyMethodId4 = "a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4";
    public const string AssemblyMethodId5 = "b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5";

    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Generate SQL INSERT statements for seed data
    /// Call this in migration Up method with migrationBuilder.Sql()
    /// </summary>
    public static string GenerateTopicSql()
    {
        return @$"
INSERT INTO catalog.""Topic"" (""Id"", ""Name"", ""Slug"", ""Description"", ""ParentId"", ""IsActive"", ""CreatedAt"", ""UpdatedAt"") VALUES
('{TopicId1}', 'Animals', 'animals', 'Animal themed 3D puzzles', NULL, true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{TopicId2}', 'Vehicles', 'vehicles', 'Vehicle and transportation themed puzzles', NULL, true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{TopicId3}', 'Architecture', 'architecture', 'Famous buildings and landmarks', NULL, true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{TopicId4}', 'Nature', 'nature', 'Natural landscapes and scenery', NULL, true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{TopicId5}', 'Fantasy', 'fantasy', 'Fantasy creatures and magical worlds', NULL, true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}');
";
    }

    public static string GenerateMaterialSql()
    {
        return @$"
INSERT INTO catalog.""Material"" (""Id"", ""Name"", ""Slug"", ""Description"", ""IsActive"", ""CreatedAt"", ""UpdatedAt"") VALUES
('{MaterialId1}', 'Wood', 'wood', 'Natural wood material', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{MaterialId2}', 'Plastic', 'plastic', 'Durable plastic material', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{MaterialId3}', 'Metal', 'metal', 'Premium metal components', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{MaterialId4}', 'Cardboard', 'cardboard', 'Eco-friendly cardboard material', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{MaterialId5}', 'Composite', 'composite', 'Advanced composite materials', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}');
";
    }

    public static string GenerateCapabilitySql()
    {
        return @$"
INSERT INTO catalog.""Capability"" (""Id"", ""Name"", ""Slug"", ""Description"", ""IsActive"", ""CreatedAt"", ""UpdatedAt"") VALUES
('{CapabilityId1}', 'Static Display', 'static-display', 'Static model for display only', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{CapabilityId2}', 'Move with Motor', 'move-with-motor', 'Model with electric motor-powered movement', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{CapabilityId3}', 'Manual Movement', 'manual-movement', 'Model operated by manual movement', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{CapabilityId4}', 'LED Light Feature', 'led-light-feature', 'Model with LED lighting effects', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{CapabilityId5}', 'Musical Gear', 'musical-gear', 'Model with musical features via rotating mechanism', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}');
";
    }

    public static string GenerateAssemblyMethodSql()
    {
        return @$"
INSERT INTO catalog.""AssemblyMethod"" (""Id"", ""Name"", ""Slug"", ""Description"", ""IsActive"", ""CreatedAt"", ""UpdatedAt"") VALUES
('{AssemblyMethodId1}', 'Snap-Fit', 'snap-fit', 'Easy snap assembly without tools', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{AssemblyMethodId2}', 'Glue Assembly', 'glue-assembly', 'Assembly using adhesive bonding', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{AssemblyMethodId3}', 'Screw Assembly', 'screw-assembly', 'Assembly using screws and bolts', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{AssemblyMethodId4}', 'Friction Fit', 'friction-fit', 'Assembly using tight fitting pieces', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}'),
('{AssemblyMethodId5}', 'Magnetic Assembly', 'magnetic-assembly', 'Assembly using magnetic connections', true, '{SeedDate:yyyy-MM-dd HH:mm:ss}', '{SeedDate:yyyy-MM-dd HH:mm:ss}');
";
    }

    /// <summary>
    /// Placeholder for extension method - seed data is applied via raw SQL in migration
    /// </summary>
    public static void SeedCatalogMasterData(this ModelBuilder modelBuilder)
    {
        // Seed data will be applied via raw SQL INSERT statements in migration
        // Use GenerateTopicSql(), GenerateMaterialSql(), GenerateCapabilitySql(), GenerateAssemblyMethodSql()
        // in migration Up method to apply seeds
    }
}
