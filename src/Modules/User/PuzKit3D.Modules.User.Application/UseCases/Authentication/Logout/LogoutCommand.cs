using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Logout;

public sealed record LogoutCommand(string UserId) : ICommand;
