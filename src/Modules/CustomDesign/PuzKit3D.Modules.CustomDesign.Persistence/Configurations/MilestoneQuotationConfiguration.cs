using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class MilestoneQuotationConfiguration : IEntityTypeConfiguration<MilestoneQuotation>
{
    public void Configure(EntityTypeBuilder<MilestoneQuotation> builder)
    {
        builder.HasKey(mq => mq.Id);

        builder.Property(mq => mq.Id)
            .HasConversion(
                id => id.Value,
                value => MilestoneQuotationId.From(value));

        builder.Property(mq => mq.ProposalId)
            .HasConversion(
                id => id.Value,
                value => ProposalId.From(value));

        builder.Property(mq => mq.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(mq => mq.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(mq => mq.CreatedAt)
            .IsRequired();

        builder.Property(mq => mq.UpdatedAt)
            .IsRequired();

        builder.HasIndex(mq => mq.Code)
            .IsUnique();

        builder.HasIndex(mq => mq.ProposalId);

        builder.HasMany(mq => mq.Details)
            .WithOne()
            .HasForeignKey(d => d.MilestoneQuotationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
