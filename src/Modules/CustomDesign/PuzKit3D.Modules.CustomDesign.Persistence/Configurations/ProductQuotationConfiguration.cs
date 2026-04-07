using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class ProductQuotationConfiguration : IEntityTypeConfiguration<ProductQuotation>
{
    public void Configure(EntityTypeBuilder<ProductQuotation> builder)
    {
        builder.HasKey(pq => pq.Id);

        builder.Property(pq => pq.Id)
            .HasConversion(
                id => id.Value,
                value => ProductQuotationId.From(value));

        builder.Property(pq => pq.ProposalId)
            .HasConversion(
                id => id.Value,
                value => ProposalId.From(value));

        builder.Property(pq => pq.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pq => pq.Volume)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pq => pq.MaterialId)
            .IsRequired();

        builder.Property(pq => pq.MaterialBasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pq => pq.BaseAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pq => pq.WeightPercent)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(pq => pq.WeightAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pq => pq.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(pq => pq.CreatedAt)
            .IsRequired();

        builder.Property(pq => pq.UpdatedAt)
            .IsRequired();

        builder.HasIndex(pq => pq.Code)
            .IsUnique();

        builder.HasIndex(pq => pq.ProposalId);
    }
}
