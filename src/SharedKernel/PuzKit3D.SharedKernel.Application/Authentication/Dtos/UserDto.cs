namespace PuzKit3D.SharedKernel.Application.Authentication.Dtos;

/// <summary>
/// Basic user information DTO
/// </summary>
public sealed record UserDto(
    string Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    bool EmailConfirmed,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? Role,
    bool IsDeleted);
