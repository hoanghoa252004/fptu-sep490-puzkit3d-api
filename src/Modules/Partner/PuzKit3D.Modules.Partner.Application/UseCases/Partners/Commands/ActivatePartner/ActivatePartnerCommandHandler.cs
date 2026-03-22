using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.ActivatePartner;

internal sealed class ActivatePartnerCommandHandler : ICommandHandler<ActivatePartnerCommand>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public ActivatePartnerCommandHandler(
        IPartnerRepository partnerRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerRepository = partnerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ActivatePartnerCommand request,
        CancellationToken cancellationToken)
    {
        var partner = await _partnerRepository.GetByIdAsync(
            PartnerId.From(request.Id),
            cancellationToken);

        if (partner is null)
        {
            return Result.Failure(PartnerError.NotFound(request.Id));
        }

        if (partner.IsActive)
        {
            return Result.Failure(PartnerError.AlreadyActive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            partner.Activate();
            _partnerRepository.Update(partner);
            return Result.Success();
        }, cancellationToken);
    }
}
