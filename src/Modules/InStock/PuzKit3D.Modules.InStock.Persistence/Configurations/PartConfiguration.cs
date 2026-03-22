using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PartId.From(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.PartType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.InstockProductId)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value))
            .IsRequired();

        builder.HasOne<InstockProduct>()
            .WithMany(p => p.Parts)
            .HasForeignKey(p => p.InstockProductId)
            .HasConstraintName("FK__instock_product__part")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.DomainEvents);
    }
}

