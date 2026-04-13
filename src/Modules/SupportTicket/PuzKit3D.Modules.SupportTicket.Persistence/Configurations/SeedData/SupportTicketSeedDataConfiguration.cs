using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Domain.Entities;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations.SeedData;

internal static class SupportTicketSeedDataConfiguration
{
    // Drive IDs (same as InStock module)
    private static readonly Guid DriveId1 = Guid.Parse("11111111-1111-1111-1111-111111111111"); // Router
    private static readonly Guid DriveId2 = Guid.Parse("22222222-2222-2222-2222-222222222222"); // Motor
    private static readonly Guid DriveId3 = Guid.Parse("33333333-3333-3333-3333-333333333333"); // Gearbox
    private static readonly Guid DriveId4 = Guid.Parse("44444444-4444-4444-4444-444444444444"); // LED
    private static readonly Guid DriveId5 = Guid.Parse("55555555-5555-5555-5555-555555555555"); // Music Box

    private static readonly DateTime SeedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void SeedReplicaDrives(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DriveReplica>().HasData(
            new
            {
                Id = DriveId1,
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
                Id = DriveId2,
                Name = "Motor",
                Description = "Electric motor drive",
                MinVolume = 50,
                QuantityInStock = 100,
                IsActive = true,
                CreatedAt = SeedDate,
                UpdatedAt = SeedDate
            },
            new
            {
                Id = DriveId3,
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
                Id = DriveId4,
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
                Id = DriveId5,
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

    public static void SeedSupportTicketMasterData(this ModelBuilder modelBuilder)
    {
        modelBuilder.SeedReplicaDrives();
    }
}
