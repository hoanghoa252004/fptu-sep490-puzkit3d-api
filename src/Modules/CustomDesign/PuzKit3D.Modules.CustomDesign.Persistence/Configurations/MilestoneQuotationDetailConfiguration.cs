using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class MilestoneQuotationDetailConfiguration : IEntityTypeConfiguration<MilestoneQuotationDetail>
{
    public void Configure(EntityTypeBuilder<MilestoneQuotationDetail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => MilestoneQuotationDetailId.From(value));

        builder.Property(d => d.MilestoneQuotationId)
            .HasConversion(
                id => id.Value,
                value => MilestoneQuotationId.From(value));

        builder.Property(d => d.MilestoneId)
            .HasConversion(
                id => id.Value,
                value => MilestoneId.From(value));

        builder.Property(d => d.LaborCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(d => d.WeightPercent)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(d => d.WeightAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(d => d.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(d => d.MilestoneQuotationId);
        builder.HasIndex(d => d.MilestoneId);
    }
}
