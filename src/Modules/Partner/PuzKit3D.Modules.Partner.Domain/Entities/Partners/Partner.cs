using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Domain.Entities.Partners;

public class Partner : AggregateRoot<PartnerId>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public string ContactEmail { get; private set; } = null!;
    public string ContactPhone { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public ImportServiceConfigId ImportServiceConfigId { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Partner(
        PartnerId id,
        string name,
        string contactEmail,
        string contactPhone,
        string address,
        string slug,
        ImportServiceConfigId importServiceConfigId,
        string? description,
        bool isActive,
        DateTime createdAt) : base(id)
    {
        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Address = address;
        Slug = slug;
        ImportServiceConfigId = importServiceConfigId;
        Description = description;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Partner() : base()
    {
    }

    public static ResultT<Partner> Create(
        string name,
        string contactEmail,
        string contactPhone,
        string address,
        string slug,
        ImportServiceConfigId importServiceConfigId,
        string? description = null,
        bool isActive = false,
        DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Partner>(PartnerError.InvalidName());

        if (name.Length > 30)
            return Result.Failure<Partner>(PartnerError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Partner>(PartnerError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure<Partner>(PartnerError.SlugTooLong(slug.Length));

        var partnerId = PartnerId.Create();
        var timestamp = createdAt ?? DateTime.UtcNow;

        var partner = new Partner(
            partnerId,
            name,
            contactEmail,
            contactPhone,
            address,
            slug,
            importServiceConfigId,
            description,
            isActive,
            timestamp);

        return Result.Success(partner);
    }

    public Result Update(
        string name,
        string contactEmail,
        string contactPhone,
        string address,
        string slug,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartnerError.InvalidName());

        if (name.Length > 30)
            return Result.Failure(PartnerError.NameTooLong(name.Length));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(PartnerError.InvalidSlug());

        if (slug.Length > 30)
            return Result.Failure(PartnerError.SlugTooLong(slug.Length));

        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        Address = address;
        Slug = slug;
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
