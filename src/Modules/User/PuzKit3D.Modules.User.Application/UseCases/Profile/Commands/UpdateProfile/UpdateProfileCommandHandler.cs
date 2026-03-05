using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateProfile;

internal sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
{
    private readonly IIdentityService _identityService;

    public UpdateProfileCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.UpdateProfileAsync(
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
