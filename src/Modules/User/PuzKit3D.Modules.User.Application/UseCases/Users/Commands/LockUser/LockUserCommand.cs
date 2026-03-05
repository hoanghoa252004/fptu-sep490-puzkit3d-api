using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.LockUser;

public sealed record LockUserCommand(string UserId) : ICommand;
