namespace PuzKit3D.SharedKernel.Application.Authentication.Dtos;

/// <summary>
/// Paginated users list response
/// </summary>
public sealed record GetUsersResponse(
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    IReadOnlyList<UserDto> Users);
