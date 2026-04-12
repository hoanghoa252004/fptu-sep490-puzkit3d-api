using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts.DomainEvents;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductDrives;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.RegularExpressions;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

public sealed partial class InstockProduct : AggregateRoot<InstockProductId>
{
    private static readonly Regex CodeRegex = CodeRegexPattern();
    private readonly List<InstockProductDrive> _drives = new();
    private readonly List<InstockProductCapabilityDetail> _capabilityDetails = new();

    public static readonly HashSet<string> ValidDifficultLevels = new()
    {
        "Basic",
        "Intermediate",
        "Advanced"
    };

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
    public Guid MaterialId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyCollection<InstockProductDrive> Drives => _drives.AsReadOnly();
    public IReadOnlyCollection<InstockProductCapabilityDetail> CapabilityDetails => _capabilityDetails.AsReadOnly();

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
        Guid materialId,
        List<Guid>? capabilityIds = null,
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

        //if (!ValidDifficultLevels.Contains(difficultLevel))
        //    return Result.Failure<InstockProduct>(InstockProductError.InvalidDifficultLevelValue(difficultLevel));

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
            materialId,
            isActive,
            timestamp);

        product.RaiseDomainEvent(new InstockProductCreatedDomainEvent(
            product.Id.Value,
            product.Code,
            product.Slug,
            product.Name,
            product.TotalPieceCount,
            product.DifficultLevel,
            product.EstimatedBuildTime,
            product.ThumbnailUrl,
            product.PreviewAsset,
            product.Description,
            product.TopicId,
            product.AssemblyMethodId,
            product.MaterialId,
            product.IsActive,
            product.CreatedAt));

        return Result.Success(product);
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
        Guid? materialId = null,
        List<Guid>? capabilityIds = null,
        string? description = null,
        bool? isActive = null)
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

            //if (!ValidDifficultLevels.Contains(difficultLevel))
            //    return Result.Failure(InstockProductError.InvalidDifficultLevelValue(difficultLevel));

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

        if (materialId.HasValue)
            MaterialId = materialId.Value;

        if (description is not null)
            Description = description;

        if (isActive.HasValue)
        {
            if (isActive.Value == IsActive)
                return Result.Failure(InstockProductError.IsActiveUnchanged(IsActive));

            IsActive = isActive.Value;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new InstockProductUpdatedDomainEvent(
            Id.Value,
            Code,
            Slug,
            Name,
            TotalPieceCount,
            DifficultLevel,
            EstimatedBuildTime,
            ThumbnailUrl,
            PreviewAsset,
            Description,
            TopicId,
            AssemblyMethodId,
            MaterialId,
            IsActive,
            UpdatedAt));

        return Result.Success();
    }


    public void Delete()
    {
        RaiseDomainEvent(new InstockProductDeletedDomainEvent(Id.Value));
    }

    public void AddDrive(Guid driveId, int quantity)
    {
        var existingDrive = _drives.FirstOrDefault(d => d.DriveId == driveId);
        if (existingDrive != null)
        {
            existingDrive.UpdateQuantity(quantity);
        }
        else
        {
            _drives.Add(InstockProductDrive.Create(Id, driveId, quantity));
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveDrive(Guid driveId)
    {
        var drive = _drives.FirstOrDefault(d => d.DriveId == driveId);
        if (drive is not null)
        {
            _drives.Remove(drive);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void SetDrives(List<(Guid driveId, int quantity)> driveList)
    {
        _drives.Clear();
        if (driveList != null && driveList.Count > 0)
        {
            foreach (var (driveId, quantity) in driveList)
            {
                _drives.Add(InstockProductDrive.Create(Id, driveId, quantity));
            }
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public List<(Guid driveId, int quantity)> GetDrives() => 
        _drives.Select(d => (d.DriveId, d.Quantity)).ToList();

    public void SetCapabilities(List<Guid> capabilityIds)
    {
        _capabilityDetails.Clear();
        if (capabilityIds != null && capabilityIds.Count > 0)
        {
            foreach (var capabilityId in capabilityIds)
            {
                _capabilityDetails.Add(InstockProductCapabilityDetail.Create(Id, capabilityId));
            }
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddCapability(Guid capabilityId)
    {
        if (!_capabilityDetails.Any(c => c.CapabilityId == capabilityId))
        {
            _capabilityDetails.Add(InstockProductCapabilityDetail.Create(Id, capabilityId));
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveCapability(Guid capabilityId)
    {
        var detail = _capabilityDetails.FirstOrDefault(c => c.CapabilityId == capabilityId);
        if (detail is not null)
        {
            _capabilityDetails.Remove(detail);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public List<Guid> GetCapabilityIds() => _capabilityDetails.Select(c => c.CapabilityId).ToList();
}

