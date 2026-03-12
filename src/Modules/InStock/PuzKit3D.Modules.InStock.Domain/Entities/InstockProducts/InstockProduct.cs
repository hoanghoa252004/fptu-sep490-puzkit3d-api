using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.RegularExpressions;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

public sealed partial class InstockProduct : AggregateRoot<InstockProductId>
{
    private static readonly Regex CodeRegex = CodeRegexPattern();
    private readonly List<Part> _parts = new();
    private readonly List<Guid> _capabilityIds = new();

    public string Code { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int TotalPieceCount { get; private set; }
    public string DifficultLevel { get; private set; } = null!;
    public int EstimatedBuildTime { get; private set; }
    public string ThumbnailUrl { get; private set; } = null!;
    public string PreviewAsset { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid TopicId { get; private set; }
    public Guid AssemblyMethodId { get; private set; }
    public Guid CapabilityId { get; private set; }
    public Guid MaterialId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<Part> Parts => _parts.AsReadOnly();
    public IReadOnlyCollection<Guid> CapabilityIds => _capabilityIds.AsReadOnly();

    [GeneratedRegex(@"^INP\d{3}$", RegexOptions.Compiled)]
    private static partial Regex CodeRegexPattern();

    private InstockProduct(
        InstockProductId id,
        string code,
        string slug,
        string name,
        int totalPieceCount,
        string difficultLevel,
        int estimatedBuildTime,
        string thumbnailUrl,
        string previewAsset,
        string? description,
        Guid topicId,
        Guid assemblyMethodId,
        Guid capabilityId,
        Guid materialId,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Code = code;
        Slug = slug;
        Name = name;
        TotalPieceCount = totalPieceCount;
        DifficultLevel = difficultLevel;
        EstimatedBuildTime = estimatedBuildTime;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Description = description;
        TopicId = topicId;
        AssemblyMethodId = assemblyMethodId;
        CapabilityId = capabilityId;
        MaterialId = materialId;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private InstockProduct() : base()
    {
    }

    public static ResultT<InstockProduct> Create(
        string code,
        string slug,
        string name,
        int totalPieceCount,
        string difficultLevel,
        int estimatedBuildTime,
        string thumbnailUrl,
        string previewAsset,
        Guid topicId,
        Guid assemblyMethodId,
        Guid capabilityId,
        Guid materialId,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidCodeFormat());

        if (code.Length > 10)
            return Result.Failure<InstockProduct>(InstockProductError.CodeTooLong(code.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<InstockProduct>(InstockProductError.SlugTooLong(slug.Length));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<InstockProduct>(InstockProductError.NameTooLong(name.Length));

        if (totalPieceCount <= 0)
            return Result.Failure<InstockProduct>(InstockProductError.InvalidTotalPieceCount());

        if (string.IsNullOrWhiteSpace(difficultLevel))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidDifficultLevel());

        if (estimatedBuildTime <= 0)
            return Result.Failure<InstockProduct>(InstockProductError.InvalidEstimatedBuildTime());

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidThumbnailUrl());

        if (string.IsNullOrWhiteSpace(previewAsset))
            return Result.Failure<InstockProduct>(InstockProductError.InvalidPreviewAsset());

        var instockProductId = InstockProductId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;
        var product = new InstockProduct(
            instockProductId,
            code,
            slug,
            name,
            totalPieceCount,
            difficultLevel,
            estimatedBuildTime,
            thumbnailUrl,
            previewAsset,
            description,
            topicId,
            assemblyMethodId,
            capabilityId,
            materialId,
            isActive,
            timestamp);

        return Result.Success(product);
    }

    public Result Update(
        string code,
        string slug,
        string name,
        int totalPieceCount,
        string difficultLevel,
        int estimatedBuildTime,
        string thumbnailUrl,
        string previewAsset,
        Guid topicId,
        Guid assemblyMethodId,
        Guid capabilityId,
        Guid materialId,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Result.Failure(InstockProductError.InvalidCode());

        if (!CodeRegex.IsMatch(code))
            return Result.Failure(InstockProductError.InvalidCodeFormat());

        if (code.Length > 10)
            return Result.Failure(InstockProductError.CodeTooLong(code.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(InstockProductError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(InstockProductError.SlugTooLong(slug.Length));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(InstockProductError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(InstockProductError.NameTooLong(name.Length));

        if (totalPieceCount <= 0)
            return Result.Failure(InstockProductError.InvalidTotalPieceCount());

        if (string.IsNullOrWhiteSpace(difficultLevel))
            return Result.Failure(InstockProductError.InvalidDifficultLevel());

        if (estimatedBuildTime <= 0)
            return Result.Failure(InstockProductError.InvalidEstimatedBuildTime());

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            return Result.Failure(InstockProductError.InvalidThumbnailUrl());

        if (string.IsNullOrWhiteSpace(previewAsset))
            return Result.Failure(InstockProductError.InvalidPreviewAsset());

        Code = code;
        Slug = slug;
        Name = name;
        TotalPieceCount = totalPieceCount;
        DifficultLevel = difficultLevel;
        EstimatedBuildTime = estimatedBuildTime;
        ThumbnailUrl = thumbnailUrl;
        PreviewAsset = previewAsset;
        Description = description;
        TopicId = topicId;
        AssemblyMethodId = assemblyMethodId;
        CapabilityId = capabilityId;
        MaterialId = materialId;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result PartialUpdate(
        string? slug = null,
        string? name = null,
        int? totalPieceCount = null,
        string? difficultLevel = null,
        int? estimatedBuildTime = null,
        string? thumbnailUrl = null,
        Dictionary<string, string>? previewAsset = null,
        Guid? topicId = null,
        Guid? assemblyMethodId = null,
        Guid? capabilityId = null,
        Guid? materialId = null,
        string? description = null)
    {
        if (slug is not null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result.Failure(InstockProductError.InvalidSlug());

            if (slug.Length > 30)
                return Result.Failure(InstockProductError.SlugTooLong(slug.Length));

            Slug = slug;
        }

        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure(InstockProductError.InvalidName());

            if (name.Length > 30)
                return Result.Failure(InstockProductError.NameTooLong(name.Length));

            Name = name;
        }

        if (totalPieceCount.HasValue)
        {
            if (totalPieceCount.Value <= 0)
                return Result.Failure(InstockProductError.InvalidTotalPieceCount());

            TotalPieceCount = totalPieceCount.Value;
        }

        if (difficultLevel is not null)
        {
            if (string.IsNullOrWhiteSpace(difficultLevel))
                return Result.Failure(InstockProductError.InvalidDifficultLevel());

            DifficultLevel = difficultLevel;
        }

        if (estimatedBuildTime.HasValue)
        {
            if (estimatedBuildTime.Value <= 0)
                return Result.Failure(InstockProductError.InvalidEstimatedBuildTime());

            EstimatedBuildTime = estimatedBuildTime.Value;
        }

        if (thumbnailUrl is not null)
        {
            if (string.IsNullOrWhiteSpace(thumbnailUrl))
                return Result.Failure(InstockProductError.InvalidThumbnailUrl());

            ThumbnailUrl = thumbnailUrl;
        }

        if (previewAsset is not null)
        {
            PreviewAsset = System.Text.Json.JsonSerializer.Serialize(previewAsset);
        }

        if (topicId.HasValue)
            TopicId = topicId.Value;

        if (assemblyMethodId.HasValue)
            AssemblyMethodId = assemblyMethodId.Value;

        if (capabilityId.HasValue)
            CapabilityId = capabilityId.Value;

        if (materialId.HasValue)
            MaterialId = materialId.Value;

        if (description is not null)
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

        RaiseDomainEvent(new InstockProductDeactivatedDomainEvent(Id.Value));
    }

    public void AddPart(Part part)
    {
        _parts.Add(part);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemovePart(Part part)
    {
        _parts.Remove(part);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddCapability(Guid capabilityId)
    {
        if (!_capabilityIds.Contains(capabilityId))
        {
            _capabilityIds.Add(capabilityId);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveCapability(Guid capabilityId)
    {
        _capabilityIds.Remove(capabilityId);
        UpdatedAt = DateTime.UtcNow;
    }
}

