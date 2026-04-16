using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductCapabilityDetailConfiguration : IEntityTypeConfiguration<InstockProductCapabilityDetail>
{
    public void Configure(EntityTypeBuilder<InstockProductCapabilityDetail> builder)
    {
        builder.ToTable("instock_product_capability_detail");

        builder.Ignore(e => e.Id);

        builder.Property(e => e.InstockProductId)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value))
            .HasColumnName("instock_product_id")
            .IsRequired();

        builder.Property(e => e.CapabilityId)
            .HasColumnName("capability_id")
            .IsRequired();

        builder.HasKey(x => new { x.InstockProductId, x.CapabilityId });

        builder.HasOne<InstockProduct>()
            .WithMany(p => p.CapabilityDetails)
            .HasForeignKey(e => e.InstockProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(e => e.DomainEvents);
    }
}
