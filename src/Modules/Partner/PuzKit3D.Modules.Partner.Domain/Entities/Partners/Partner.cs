using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners.DomainEvents;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.RegularExpressions;

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
        // Name
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Partner>(PartnerError.EmptyName());

        if (name.Length > 30)
            return Result.Failure<Partner>(PartnerError.NameTooLong(name.Length));

        // ContactEmail
        if (string.IsNullOrWhiteSpace(contactEmail))
            return Result.Failure<Partner>(PartnerError.EmptyEmail());

        if (!contactEmail.Contains("@"))
            return Result.Failure<Partner>(PartnerError.InvalidEmail());

        // ContactPhone
        if (string.IsNullOrWhiteSpace(contactPhone))
            return Result.Failure<Partner>(PartnerError.EmptyPhone());
        var phoneRegex = @"^0\d{9}$";

        if (!Regex.IsMatch(contactPhone, phoneRegex))
            return Result.Failure<Partner>(PartnerError.InvalidPhone());
        // Address
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Partner>(PartnerError.EmptyAddress());

        // Slug
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Partner>(PartnerError.EmptySlug());

        if (slug.Length > 30)
            return Result.Failure<Partner>(PartnerError.SlugTooLong(slug.Length));

        if (slug.Trim().Contains(" "))
            return Result.Failure<Partner>(PartnerError.InvalidSlug());

        // ImportServiceConfigId
        if (importServiceConfigId is null)
            return Result.Failure<Partner>(PartnerError.EmptyImportServiceConfig());

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
        ImportServiceConfigId importServiceConfigId,
        string name,
        string contactEmail,
        string contactPhone,
        string address,
        string slug,
        string? description = null)
    {
        // ImportServiceConfigId
        if (importServiceConfigId is null)
            return Result.Failure(PartnerError.EmptyImportServiceConfig());

        // Name
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(PartnerError.EmptyName());

        if (name.Length > 30)
            return Result.Failure(PartnerError.NameTooLong(name.Length));

        // ContactEmail
        if (string.IsNullOrWhiteSpace(contactEmail))
            return Result.Failure(PartnerError.EmptyEmail());

        if (!contactEmail.Contains("@"))
            return Result.Failure(PartnerError.InvalidEmail());

        // ContactPhone
        if (string.IsNullOrWhiteSpace(contactPhone))
            return Result.Failure(PartnerError.EmptyPhone());

        var phoneRegex = @"^0\d{9}$";

        if (!Regex.IsMatch(contactPhone, phoneRegex))
            return Result.Failure(PartnerError.InvalidPhone());

        // Address
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure(PartnerError.EmptyAddress());

        // Slug
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(PartnerError.EmptySlug());

        if (slug.Length > 30)
            return Result.Failure(PartnerError.SlugTooLong(slug.Length));

        if (slug.Trim().Contains(" "))
            return Result.Failure(PartnerError.InvalidSlug());

        ImportServiceConfigId = importServiceConfigId;
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
        RaiseDomainEvent(new PartnerDeletedDomainEvent(Id.Value, UpdatedAt));
    }
}
