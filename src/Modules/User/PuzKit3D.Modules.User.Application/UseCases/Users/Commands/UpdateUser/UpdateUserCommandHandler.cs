using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.UpdateUserAsync(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.ProvinceId,
            request.ProvinceName,
            request.DistrictId,
            request.DistrictName,
            request.WardCode,
            request.WardName,
            request.StreetAddress,
            cancellationToken);
    }
}
