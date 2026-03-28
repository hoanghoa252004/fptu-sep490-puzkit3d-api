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
        if (partner.Slug != request.Slug 
            || partner.Name != request.Name 
            || partner.ContactEmail != request.ContactEmail 
            || partner.ContactPhone != request.ContactPhone)
        {
            Domain.Entities.Partners.Partner? existingPartner;
            existingPartner = await _partnerRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingPartner is not null)
            {
                return Result.Failure(PartnerError.DuplicateSlug(request.Slug));
            }

            existingPartner = await _partnerRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingPartner is not null)
            {
                return Result.Failure(PartnerError.DuplicateName(request.Name));
            }

            existingPartner = await _partnerRepository.GetByEmailAsync(request.ContactEmail, cancellationToken);
            if (existingPartner is not null)
            {
                return Result.Failure(PartnerError.DuplicateEmail(request.ContactEmail));
            }

            existingPartner = await _partnerRepository.GetByPhoneAsync(request.ContactPhone, cancellationToken);
            if (existingPartner is not null)
            {
                return Result.Failure(PartnerError.DuplicatePhone(request.ContactPhone));
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
