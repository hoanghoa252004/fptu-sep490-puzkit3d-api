using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(string UserId) : ICommand;
