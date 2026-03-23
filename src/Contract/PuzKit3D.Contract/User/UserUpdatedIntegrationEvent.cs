using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.User;

public sealed record UserUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid UserId,
    string Email,
    string PasswordHash,
    Guid RoleId,
    string FullName,
    DateTime? DateOfBirth,
    string PhoneNumber,
    string? Province,
    string? District,
    string? Ward,
    string? StreetAddress
) : IIntegrationEvent;

