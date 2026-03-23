namespace PuzKit3D.SharedKernel.Application.Authentication.Dtos;

/// <summary>
/// Detailed user information DTO including address
/// </summary>
public sealed record UserDetailDto(
    string Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    bool EmailConfirmed,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? ProvinceName,
    string? DistrictName,
    string? WardName,
    string? StreetAddress,
    string? Role,
    bool IsDeleted);
