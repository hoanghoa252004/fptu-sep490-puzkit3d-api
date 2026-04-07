using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
{
    public void Configure(EntityTypeBuilder<Milestone> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MilestoneId.From(value));

        builder.Property(m => m.Name)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasColumnType("text");

        builder.Property(m => m.SequenceOrder)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasIndex(m => m.SequenceOrder)
            .IsUnique();

        builder.HasMany(m => m.Phases)
            .WithOne()
            .HasForeignKey(p => p.MilestoneId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
