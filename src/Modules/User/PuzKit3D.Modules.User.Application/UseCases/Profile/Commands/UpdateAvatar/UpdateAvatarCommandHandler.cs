using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateAvatar;

internal sealed class UpdateAvatarCommandHandler : ICommandHandler<UpdateAvatarCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateAvatarCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        UpdateAvatarCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.UpdateAvatarAsync(
            request.UserId,
            request.AvatarUrl,
            cancellationToken);
    }
}
