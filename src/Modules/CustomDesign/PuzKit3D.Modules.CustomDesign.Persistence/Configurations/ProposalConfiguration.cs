using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
{
    public void Configure(EntityTypeBuilder<Proposal> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => ProposalId.From(value));

        builder.Property(p => p.RequestId)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequestId.From(value));

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.LaborCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.ProductCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.TotalWeightPercent)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(p => p.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.ManagerApprovedAt)
            .IsRequired(false);

        builder.Property(p => p.CustomerApprovedAt)
            .IsRequired(false);

        builder.Property(p => p.Note)
            .HasColumnType("text");

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.HasIndex(p => p.RequestId);
        builder.HasIndex(p => p.Status);

        // Relationships with collections - using HasMany without Navigation properties on child
        builder.HasMany(p => p.MilestoneQuotations)
            .WithOne()
            .HasForeignKey(mq => mq.ProposalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductQuotations)
            .WithOne()
            .HasForeignKey(pq => pq.ProposalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Workflows)
            .WithOne()
            .HasForeignKey(w => w.ProposalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
