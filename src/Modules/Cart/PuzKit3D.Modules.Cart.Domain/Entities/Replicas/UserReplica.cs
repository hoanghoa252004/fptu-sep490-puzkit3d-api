using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

public sealed class UserReplica : Entity<Guid>
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Guid RoleId { get; private set; }
    public string FullName { get; private set; } = null!;
    public DateTime? DateOfBirth { get; private set; }
    public string Address { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;

    private UserReplica(
        Guid id,
        string email,
        string passwordHash,
        Guid roleId,
        string fullName,
        DateTime? dateOfBirth,
        string address,
        string phoneNumber) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Address = address;
        PhoneNumber = phoneNumber;
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
        string address,
        string phoneNumber)
    {
        return new UserReplica(
            id,
            email,
            passwordHash,
            roleId,
            fullName,
            dateOfBirth,
            address,
            phoneNumber);
    }

    public void Update(
        string email,
        string fullName,
        DateTime? dateOfBirth,
        string address,
        string phoneNumber)
    {
        Email = email;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Address = address;
        PhoneNumber = phoneNumber;
    }
}
