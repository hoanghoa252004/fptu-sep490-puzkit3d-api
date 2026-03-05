using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ProvinceId,
    string? ProvinceName,
    string? DistrictId,
    string? DistrictName,
    string? WardCode,
    string? WardName,
    string? StreetAddress) : ICommand;
