using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommandT<AuthenticationResult>;
