using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class PieceConfiguration : IEntityTypeConfiguration<Piece>
{
    public void Configure(EntityTypeBuilder<Piece> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PieceId.From(value));

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.PartId)
            .HasConversion(
                id => id.Value,
                value => PartId.From(value))
            .IsRequired();

        builder.HasOne<Part>()
            .WithMany(p => p.Pieces)
            .HasForeignKey(p => p.PartId)
            .HasConstraintName("FK__part__piece")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.DomainEvents);
    }
}
