using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductDriveConfiguration : IEntityTypeConfiguration<InstockProductDrive>
{
    public void Configure(EntityTypeBuilder<InstockProductDrive> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => InstockProductDriveId.From(value));

        builder.Property(p => p.InstockProductId)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value))
            .IsRequired();

        builder.Property(p => p.DriveId)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.HasOne<InstockProduct>()
            .WithMany(p => p.Drives)
            .HasForeignKey(p => p.InstockProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.InstockProductId, p.DriveId })
            .IsUnique()
            .HasDatabaseName("UK__instock_product_drive__product_drive");
    }
}
