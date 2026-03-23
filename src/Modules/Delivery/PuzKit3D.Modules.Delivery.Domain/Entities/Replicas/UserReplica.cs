using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

public sealed class UserReplica : Entity<Guid>
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public string FullName { get; private set; } = null!;
    public DateTime? DateOfBirth { get; private set; }
    public string PhoneNumber { get; private set; } = null!;
    
    // Address properties
    public string? Province { get; private set; }
    public string? District { get; private set; }
    public string? Ward { get; private set; }
    public string? StreetAddress { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private UserReplica(
        Guid id,
        string email,
        string passwordHash,
        Guid roleId,
        string fullName,
        DateTime? dateOfBirth,
        string phoneNumber,
        string? province,
        string? district,
        string? ward,
        string? streetAddress,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Province = province;
        District = district;
        Ward = ward;
        StreetAddress = streetAddress;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private UserReplica() : base()
    {
    }

    public static UserReplica Create(
        Guid id,
        string email,
        string passwordHash,
        Guid roleId,
        string fullName,
        DateTime? dateOfBirth,
        string phoneNumber,
        string? province,
        string? district,
        string? ward,
        string? streetAddress)
    {
        return new UserReplica(
            id,
            email,
            passwordHash,
            roleId,
            fullName,
            dateOfBirth,
            phoneNumber,
            province,
            district,
            ward,
            streetAddress,
            DateTime.UtcNow,
            DateTime.UtcNow);
    }

    public void Update(
        string email,
        string passwordHash,
        Guid roleId,
        string fullName,
        DateTime? dateOfBirth,
        string phoneNumber,
        string? province,
        string? district,
        string? ward,
        string? streetAddress)
    {
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Province = province;
        District = district;
        Ward = ward;
        StreetAddress = streetAddress;
        UpdatedAt = DateTime.UtcNow;
    }
}
