using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductAssemblyMethodDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductAssemblyMethodDetailConfiguration : IEntityTypeConfiguration<InstockProductAssemblyMethodDetail>
{
    public void Configure(EntityTypeBuilder<InstockProductAssemblyMethodDetail> builder)
    {
        builder.Ignore(e => e.Id);

        builder.Property(e => e.InstockProductId)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value))
            .HasColumnName("instock_product_id")
            .IsRequired();

        builder.Property(x => x.AssemblyMethodId)
            .IsRequired();

        builder.HasKey(x => new { x.InstockProductId, x.AssemblyMethodId });

        builder.HasOne<InstockProduct>()
            .WithMany(p => p.AssemblyMethodDetails)
            .HasForeignKey(e => e.InstockProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}
