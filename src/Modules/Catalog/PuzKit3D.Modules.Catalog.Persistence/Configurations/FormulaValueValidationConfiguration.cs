using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class FormulaValueValidationConfiguration : IEntityTypeConfiguration<FormulaValueValidation>
{
    public void Configure(EntityTypeBuilder<FormulaValueValidation> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Value,
                value => FormulaValueValidationId.From(value));

        builder.Property(f => f.FormulaId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => FormulaId.From(value));

        builder.Property(f => f.MinValue)
            .IsRequired()
            .HasPrecision(5, 4);

        builder.Property(f => f.MaxValue)
            .IsRequired()
            .HasPrecision(5, 4);

        builder.Property(f => f.Output)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(f => f.UpdatedAt)
            .IsRequired();
    }
}
