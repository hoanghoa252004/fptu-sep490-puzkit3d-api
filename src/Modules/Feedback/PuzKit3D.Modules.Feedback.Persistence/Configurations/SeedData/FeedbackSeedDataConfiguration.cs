using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations.SeedData;

internal static class FeedbackSeedDataConfiguration
{
    // Same product IDs as InStock module
    private static readonly Guid ProductId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid ProductId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid ProductId3 = Guid.Parse("10000000-0000-0000-0000-000000000003");
    private static readonly Guid ProductId4 = Guid.Parse("10000000-0000-0000-0000-000000000004");
    private static readonly Guid ProductId5 = Guid.Parse("10000000-0000-0000-0000-000000000005");
    private static readonly Guid ProductId6 = Guid.Parse("10000000-0000-0000-0000-000000000006");
    private static readonly Guid ProductId7 = Guid.Parse("10000000-0000-0000-0000-000000000007");
    private static readonly Guid ProductId8 = Guid.Parse("10000000-0000-0000-0000-000000000008");
    private static readonly Guid ProductId9 = Guid.Parse("10000000-0000-0000-0000-000000000009");
    private static readonly Guid ProductId10 = Guid.Parse("10000000-0000-0000-0000-000000000010");

    public static void SeedProductReplicas(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductReplica>().HasData(
            new
            {
                Id = ProductId1,
                Type = "Instock",
                Name = "UGT-24 Endurance Racer"
            },
            new
            {
                Id = ProductId2,
                Type = "Instock",
                Name = "Mad Hornet Airplane"
            },
            new
            {
                Id = ProductId3,
                Type = "Instock",
                Name = "Eagle 3D Puzzle"
            },
            new
            {
                Id = ProductId4,
                Type = "Instock",
                Name = "Sports Car 3D Puzzle"
            },
            new
            {
                Id = ProductId5,
                Type = "Instock",
                Name = "Airplane 3D Puzzle"
            },
            new
            {
                Id = ProductId6,
                Type = "Instock",
                Name = "Motorcycle 3D Puzzle"
            },
            new
            {
                Id = ProductId7,
                Type = "Instock",
                Name = "Tiger 3D Puzzle"
            },
            new
            {
                Id = ProductId8,
                Type = "Instock",
                Name = "Dolphin 3D Puzzle"
            },
            new
            {
                Id = ProductId9,
                Type = "Instock",
                Name = "Helicopter 3D Puzzle"
            },
            new
            {
                Id = ProductId10,
                Type = "Instock",
                Name = "Dragon 3D Puzzle"
            }
        );
    }
}
