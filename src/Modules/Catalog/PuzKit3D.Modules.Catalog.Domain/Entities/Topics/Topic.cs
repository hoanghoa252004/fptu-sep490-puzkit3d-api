using PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

public class Topic : AggregateRoot<TopicId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public TopicId? ParentId { get; private set; }
    public decimal FactorPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation properties
    public ICollection<TopicMaterialCapability> TopicMaterialCapabilities { get; private set; } = new List<TopicMaterialCapability>();

    private Topic(
        TopicId id,
        string name,
        string slug,
        TopicId? parentId,
        decimal factorPercentage,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Slug = slug;
        ParentId = parentId;
        FactorPercentage = factorPercentage;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Topic() : base()
    {
    }

    public static ResultT<Topic> Create(
        string name,
        string slug,
        TopicId? parentId,
        decimal factorPercentage,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Topic>(TopicError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Topic>(TopicError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Topic>(TopicError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<Topic>(TopicError.SlugTooLong(slug.Length));

        var topicId = TopicId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var topic = new Topic(
            topicId,
            name,
            slug,
            parentId,
            factorPercentage,
            description,
            isActive,
            timestamp);

        topic.RaiseDomainEvent(new TopicCreatedDomainEvent(
            topic.Id.Value,
            topic.Name,
            topic.Slug,
            topic.ParentId?.Value,
            topic.FactorPercentage,
            topic.Description,
            topic.IsActive,
            topic.CreatedAt));

        return Result.Success(topic);
    }

    public Result Update(string? name = null, string? slug = null, TopicId? parentId = null, decimal? factorPercentage = null, string? description = null, bool? isActive = null)
    {
        // Validate only provided fields
        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(TopicError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(TopicError.NameTooLong(name.Length));

            Name = name;
        }

        if (slug != null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result.Failure(TopicError.InvalidSlug());

            if (slug.Length > 30)
                return Result.Failure(TopicError.SlugTooLong(slug.Length));

            Slug = slug;
        }

        if (parentId != null)
        {
            ParentId = parentId;
        }

        if (factorPercentage.HasValue)
        {
            FactorPercentage = factorPercentage.Value;
        }

        if (description != null)
        {
            Description = description;
        }

        if (isActive.HasValue && isActive.Value != IsActive)
        {
            IsActive = isActive.Value;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new TopicUpdatedDomainEvent(
            Id.Value,
            Name,
            Slug,
            ParentId?.Value,
            FactorPercentage,
            Description,
            UpdatedAt,
            IsActive));

        return Result.Success();
    }

    public void Delete()
    {
        RaiseDomainEvent(new TopicDeletedDomainEvent(
            Id.Value,
            DateTime.UtcNow));
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
