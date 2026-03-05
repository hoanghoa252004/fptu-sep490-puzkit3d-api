using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.LockUser;

internal sealed class LockUserCommandHandler : ICommandHandler<LockUserCommand>
{
    private readonly IIdentityService _identityService;

    public LockUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        LockUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.LockUserAsync(
            request.UserId,
            cancellationToken);
    }
}
