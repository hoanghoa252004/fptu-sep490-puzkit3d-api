using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand;
