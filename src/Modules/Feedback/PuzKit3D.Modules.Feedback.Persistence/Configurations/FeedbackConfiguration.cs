using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FeedbackEntity = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.Feedback;
using FeedbackIdType = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.FeedbackId;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations;

internal sealed class FeedbackConfiguration : IEntityTypeConfiguration<FeedbackEntity>
{
    public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Value,
                value => FeedbackIdType.From(value));

        builder.Property(f => f.OrderId)
            .IsRequired();

        builder.Property(f => f.UserId)
            .IsRequired();

        builder.Property(f => f.Rating)
            .IsRequired();

        builder.Property(f => f.Comment)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .IsRequired();

        builder.HasIndex(f => new { f.OrderId, f.UserId })
            .IsUnique();

        builder.HasIndex(f => f.UserId);
    }
}

