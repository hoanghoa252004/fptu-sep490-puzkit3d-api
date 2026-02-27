using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string Role,
    string? FirstName,
    string? LastName) : ICommandT<string>;
