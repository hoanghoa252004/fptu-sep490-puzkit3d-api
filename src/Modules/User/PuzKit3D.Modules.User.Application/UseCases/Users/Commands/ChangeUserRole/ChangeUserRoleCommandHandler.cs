using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ChangeUserRole;

internal sealed class ChangeUserRoleCommandHandler : ICommandHandler<ChangeUserRoleCommand>
{
    private readonly IIdentityService _identityService;

    public ChangeUserRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        ChangeUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.ChangeUserRoleAsync(
            request.UserId,
            request.NewRole,
            cancellationToken);
    }
}
