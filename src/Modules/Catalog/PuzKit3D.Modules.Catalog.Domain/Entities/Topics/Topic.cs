using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

public class Topic : AggregateRoot<TopicId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = null!;
    public TopicId ParentId { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Topic(
        TopicId id,
        string name,
        string slug,
        TopicId parentId,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        Slug = slug;
        ParentId = parentId;
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
        TopicId parentId,
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
            description,
            isActive,
            timestamp);

        return Result.Success(topic);
    }

    public Result Update(string name, string slug, TopicId parentId, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(TopicError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(TopicError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(TopicError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(TopicError.SlugTooLong(slug.Length));

        Name = name;
        Slug = slug;
        ParentId = parentId;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
