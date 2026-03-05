using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
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
