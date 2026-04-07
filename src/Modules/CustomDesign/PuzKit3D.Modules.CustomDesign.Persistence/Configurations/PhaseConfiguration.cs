using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class PhaseConfiguration : IEntityTypeConfiguration<Phase>
{
    public void Configure(EntityTypeBuilder<Phase> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PhaseId.From(value));

        builder.Property(p => p.MilestoneId)
            .HasConversion(
                id => id.Value,
                value => MilestoneId.From(value));

        builder.Property(p => p.Name)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnType("text");

        builder.Property(p => p.SequenceOrder)
            .IsRequired();

        builder.Property(p => p.BasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.SequenceOrder)
            .IsUnique();

        builder.HasIndex(p => p.MilestoneId);
    }
}
