using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class CartConfiguration : IEntityTypeConfiguration<Domain.Entities.Carts.Cart>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Carts.Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CartId.From(value));

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.CartType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.TotalItem)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(c => new { c.UserId, c.CartType })
            .IsUnique()
            .HasDatabaseName("CUK___cart___user_id__cart_type");

        builder.Navigation(c => c.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(c => c.DomainEvents);
    }
}
