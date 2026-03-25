using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.UpdatePartner;

internal sealed class UpdatePartnerCommandHandler : ICommandHandler<UpdatePartnerCommand>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public UpdatePartnerCommandHandler(
        IPartnerRepository partnerRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerRepository = partnerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdatePartnerCommand request,
        CancellationToken cancellationToken)
    {
        var partner = await _partnerRepository.GetByIdAsync(
            PartnerId.From(request.Id),
            cancellationToken);

        if (partner is null)
        {
            return Result.Failure(PartnerError.NotFound(request.Id));
        }

        // Check if new slug is already used by another partner
        if (partner.Slug != request.Slug)
        {
            var existingPartnerWithSlug = await _partnerRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingPartnerWithSlug is not null)
            {
                return Result.Failure(
                    Error.Conflict("partner.slug_already_exists", $"Partner with slug '{request.Slug}' already exists."));
            }
        }

        var updateResult = partner.Update(
            ImportServiceConfigId.From(request.ImportServiceConfigId),
            request.Name,
            request.ContactEmail,
            request.ContactPhone,
            request.Address,
            request.Slug,
            request.Description);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            _partnerRepository.Update(partner);
            return Result.Success();
        }, cancellationToken);
    }
}
