using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommandT<AuthenticationResult>;
