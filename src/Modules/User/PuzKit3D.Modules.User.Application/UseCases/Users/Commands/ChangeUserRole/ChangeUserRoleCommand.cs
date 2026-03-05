using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ChangeUserRole;

public sealed record ChangeUserRoleCommand(string UserId, string NewRole) : ICommand;
