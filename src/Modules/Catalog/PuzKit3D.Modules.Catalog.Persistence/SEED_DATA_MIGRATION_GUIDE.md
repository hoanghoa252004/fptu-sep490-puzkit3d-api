# Catalog Master Data Seeding Guide

## Overview
Master data (Topic, Material, Capability, AssemblyMethod) seeding is handled via raw SQL in migrations to avoid Value Object conversion issues.

## Helper Methods Available

The `CatalogSeedDataConfiguration` class provides helper methods to generate SQL INSERT statements:

- `GenerateTopicSql()` - Generate INSERT for Topics
- `GenerateMaterialSql()` - Generate INSERT for Materials  
- `GenerateCapabilitySql()` - Generate INSERT for Capabilities
- `GenerateAssemblyMethodSql()` - Generate INSERT for AssemblyMethods

## How to Use in Migration

### Step 1: Create a new migration
```bash
dotnet ef migrations add SeedCatalogMasterData -p src/Modules/Catalog/PuzKit3D.Modules.Catalog.Persistence -s src/WebApi/PuzKit3D.WebApi
```

### Step 2: Edit the migration Up() method

In your migration file (e.g., `20260313_SeedCatalogMasterData.cs`), add the seed SQL in the `Up()` method:

```csharp
using PuzKit3D.Modules.Catalog.Persistence.Configurations.SeedData;

protected override void Up(MigrationBuilder migrationBuilder)
{
    // Apply seed data via raw SQL
    migrationBuilder.Sql(CatalogSeedDataConfiguration.GenerateTopicSql());
    migrationBuilder.Sql(CatalogSeedDataConfiguration.GenerateMaterialSql());
    migrationBuilder.Sql(CatalogSeedDataConfiguration.GenerateCapabilitySql());
    migrationBuilder.Sql(CatalogSeedDataConfiguration.GenerateAssemblyMethodSql());
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    // Delete seed data when rolling back
    migrationBuilder.Sql(@"
        DELETE FROM catalog.""Topic"";
        DELETE FROM catalog.""Material"";
        DELETE FROM catalog.""Capability"";
        DELETE FROM catalog.""AssemblyMethod"";
    ");
}
```

## Master Data IDs

All IDs are hard-coded constants in `CatalogSeedDataConfiguration`:

### Topics (5)
- `TopicId1`: `a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1` - Animals
- `TopicId2`: `b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2` - Vehicles
- `TopicId3`: `c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3` - Architecture
- `TopicId4`: `d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4` - Nature
- `TopicId5`: `e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5` - Fantasy

### Materials (5)
- `MaterialId1`: `f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1` - Wood
- `MaterialId2`: `a2a2a2a2-a2a2-a2a2-a2a2-a2a2a2a2a2a2` - Plastic
- `MaterialId3`: `b3b3b3b3-b3b3-b3b3-b3b3-b3b3b3b3b3b3` - Metal
- `MaterialId4`: `c4c4c4c4-c4c4-c4c4-c4c4-c4c4c4c4c4c4` - Cardboard
- `MaterialId5`: `d5d5d5d5-d5d5-d5d5-d5d5-d5d5d5d5d5d5` - Composite

### Capabilities (5)
- `CapabilityId1`: `e1e1e1e1-e1e1-e1e1-e1e1-e1e1e1e1e1e1` - Static Display
- `CapabilityId2`: `f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2` - Move with Motor
- `CapabilityId3`: `a3a3a3a3-a3a3-a3a3-a3a3-a3a3a3a3a3a3` - Manual Movement
- `CapabilityId4`: `b4b4b4b4-b4b4-b4b4-b4b4-b4b4b4b4b4b4` - LED Light Feature
- `CapabilityId5`: `c5c5c5c5-c5c5-c5c5-c5c5-c5c5c5c5c5c5` - Musical Gear

### Assembly Methods (5)
- `AssemblyMethodId1`: `d1d1d1d1-d1d1-d1d1-d1d1-d1d1d1d1d1d1` - Snap-Fit
- `AssemblyMethodId2`: `e2e2e2e2-e2e2-e2e2-e2e2-e2e2e2e2e2e2` - Glue Assembly
- `AssemblyMethodId3`: `f3f3f3f3-f3f3-f3f3-f3f3-f3f3f3f3f3f3` - Screw Assembly
- `AssemblyMethodId4`: `a4a4a4a4-a4a4-a4a4-a4a4-a4a4a4a4a4a4` - Friction Fit
- `AssemblyMethodId5`: `b5b5b5b5-b5b5-b5b5-b5b5-b5b5b5b5b5b5` - Magnetic Assembly

## InStock Module Synchronization

The InStock module uses replica tables with the **exact same IDs** for data consistency:
- InStock `TopicReplica` uses the same Topic IDs
- InStock `MaterialReplica` uses the same Material IDs
- InStock `CapabilityReplica` uses the same Capability IDs
- InStock `AssemblyMethodReplica` uses the same AssemblyMethod IDs

**Important:** Apply Catalog seed data migration BEFORE InStock to ensure referential integrity.

## Notes

- All IDs are public constants for easy reference in code
- Seed date: `2026-01-01 00:00:00 UTC`
- All records are created with `IsActive = true`
