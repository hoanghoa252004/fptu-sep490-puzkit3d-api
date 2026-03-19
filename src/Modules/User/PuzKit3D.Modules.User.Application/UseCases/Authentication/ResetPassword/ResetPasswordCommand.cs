using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(
    string UserId,
    string Token,
    string NewPassword
) : ICommand;
