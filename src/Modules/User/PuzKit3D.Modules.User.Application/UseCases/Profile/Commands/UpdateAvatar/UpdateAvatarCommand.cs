using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateAvatar;

public sealed record UpdateAvatarCommand(string UserId, string AvatarUrl) : ICommand;
