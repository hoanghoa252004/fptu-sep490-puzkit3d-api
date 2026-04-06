using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductOrderDetailConfiguration : IEntityTypeConfiguration<PartnerProductOrderDetail>
{
    public void Configure(EntityTypeBuilder<PartnerProductOrderDetail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductOrderDetailId.From(value));

        builder.Property(d => d.PartnerProductOrderId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductOrderId.From(value))
            .IsRequired();

        builder.Property(d => d.PartnerProductId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductId.From(value))
            .IsRequired();

        builder.Property(d => d.PartnerProductName)
            .HasMaxLength(30);

        builder.Property(d => d.UnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.TotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.HasIndex(d => new { d.PartnerProductOrderId, d.PartnerProductId })
            .IsUnique();

        //builder.HasOne<PartnerProductOrder>()
        //    .WithMany()
        //    .HasForeignKey(d => d.PartnerProductOrderId)
        //    .OnDelete(DeleteBehavior.Restrict)
        //    .IsRequired();

        builder.HasOne<PartnerProduct>()
            .WithMany()
            .HasForeignKey(d => d.PartnerProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
