using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class FormulaConfiguration : IEntityTypeConfiguration<Formula>
{
    public void Configure(EntityTypeBuilder<Formula> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Value,
                value => FormulaId.From(value));

        builder.Property(f => f.Code)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                code => code.ToString(),
                value => Enum.Parse<FormulaCode>(value));

        builder.Property(f => f.Expression)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(f => f.Description)
            .HasColumnType("text");

        builder.Property(f => f.IsNeedValidation)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(f => f.UpdatedAt)
            .IsRequired();

        builder.HasMany(f => f.FormulaValueValidations)
            .WithOne()
            .HasForeignKey(fvv => fvv.FormulaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


