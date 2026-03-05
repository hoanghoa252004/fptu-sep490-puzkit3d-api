using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.UnlockUser;

public sealed record UnlockUserCommand(string UserId) : ICommand;
