using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.SupportTicket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations;

internal sealed class DriveReplicaConfiguration : IEntityTypeConfiguration<DriveReplica>
{
    public void Configure(EntityTypeBuilder<DriveReplica> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(d => d.Description)
            .HasColumnType("text");

        builder.Property(d => d.MinVolume)
            .IsRequired(false);

        builder.Property(d => d.QuantityInStock)
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();
    }
}

