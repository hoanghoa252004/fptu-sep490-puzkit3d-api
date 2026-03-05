using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ActivateUser;

public sealed record ActivateUserCommand(string UserId) : ICommand;
