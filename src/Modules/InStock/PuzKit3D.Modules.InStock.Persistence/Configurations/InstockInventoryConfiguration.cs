using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockInventoryConfiguration : IEntityTypeConfiguration<InstockInventory>
{
    public void Configure(EntityTypeBuilder<InstockInventory> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(
                id => id.Value,
                value => InstockInventoryId.From(value));

        builder.Property(i => i.InstockProductVariantId)
            .HasConversion(
                id => id.Value,
                value => InstockProductVariantId.From(value))
            .IsRequired();

        builder.Property(i => i.TotalQuantity)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .IsRequired();

        builder.HasOne<InstockProductVariant>()
            .WithMany()
            .HasForeignKey(i => i.InstockProductVariantId)
            .HasConstraintName("FK__instock_product_variant__instock_inventory")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(i => i.DomainEvents);
    }
}
