using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.From(value))
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("price");

        builder.Property(p => p.Stock)
            .IsRequired()
            .HasColumnName("stock");

        builder.HasIndex(p => p.Name)
            .IsUnique();
    }
}
