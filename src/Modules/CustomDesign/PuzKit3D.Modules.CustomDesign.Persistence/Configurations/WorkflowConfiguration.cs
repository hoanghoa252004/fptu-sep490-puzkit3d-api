using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(
                id => id.Value,
                value => WorkflowId.From(value));

        builder.Property(w => w.ProposalId)
            .HasConversion(
                id => id.Value,
                value => ProposalId.From(value));

        builder.Property(w => w.PhaseId)
            .HasConversion(
                id => id.Value,
                value => PhaseId.From(value));

        builder.Property(w => w.StartDate)
            .IsRequired();

        builder.Property(w => w.EndDate)
            .IsRequired(false);

        builder.Property(w => w.Description)
            .HasColumnType("text");

        builder.Property(w => w.Outcome)
            .HasColumnType("text");

        builder.Property(w => w.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();

        builder.HasIndex(w => w.ProposalId);
        builder.HasIndex(w => w.PhaseId);
        builder.HasIndex(w => w.Status);
    }
}
