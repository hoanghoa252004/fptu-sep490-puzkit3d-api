using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string? FirstName,
    string? LastName) : ICommandT<string>;
