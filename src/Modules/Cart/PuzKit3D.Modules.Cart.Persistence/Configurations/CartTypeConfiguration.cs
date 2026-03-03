using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class CartTypeConfiguration : IEntityTypeConfiguration<CartType>
{
    public void Configure(EntityTypeBuilder<CartType> builder)
    {
        builder.ToTable("cart_type");

        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.Id)
            .HasConversion(
                id => id.Value,
                value => CartTypeId.From(value))
            .HasColumnName("id");

        builder.Property(ct => ct.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(ct => ct.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(ct => ct.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(ct => ct.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(ct => ct.Name)
            .IsUnique()
            .HasDatabaseName("UK___cart_type_name");

        builder.Ignore(ct => ct.DomainEvents);

        // Seed initial cart types
        builder.HasData(
            new
            {
                Id = CartTypeId.From(Guid.Parse("11111111-1111-1111-1111-111111111111")),
                Name = "INSTOCK",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = CartTypeId.From(Guid.Parse("22222222-2222-2222-2222-222222222222")),
                Name = "PARTNER",
                IsActive = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
