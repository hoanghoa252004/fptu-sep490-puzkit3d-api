using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ActivateUser;

internal sealed class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand>
{
    private readonly IIdentityService _identityService;

    public ActivateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.ActivateUserAsync(
            request.UserId,
            cancellationToken);
    }
}
