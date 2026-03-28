using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.CreatePartner;

internal sealed class CreatePartnerCommandHandler : ICommandTHandler<CreatePartnerCommand, Guid>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public CreatePartnerCommandHandler(
        IPartnerRepository partnerRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerRepository = partnerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreatePartnerCommand request,
        CancellationToken cancellationToken)
    {
        Domain.Entities.Partners.Partner? existingPartner;

        // Check if slug already exists
        existingPartner = await _partnerRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingPartner is not null)
        {
            return Result.Failure<Guid>(PartnerError.DuplicateSlug(request.Slug));
        }

        // Check if name already exists
        existingPartner = await _partnerRepository.GetByNameAsync(request.Name, cancellationToken);
        if (existingPartner is not null)
        {
            return Result.Failure<Guid>(PartnerError.DuplicateName(request.Name));
        }

        // Check if contact email already exists
        existingPartner = await _partnerRepository.GetByEmailAsync(request.ContactEmail, cancellationToken);
        if (existingPartner is not null)
        {
            return Result.Failure<Guid>(PartnerError.DuplicateEmail(request.ContactEmail));
        }

        // Check if contact phone already exists
        existingPartner = await _partnerRepository.GetByPhoneAsync(request.ContactPhone, cancellationToken);
        if (existingPartner is not null)
        {
            return Result.Failure<Guid>(PartnerError.DuplicatePhone(request.ContactPhone));
        }

        var partnerResult = Domain.Entities.Partners.Partner.Create(
            request.Name,
            request.ContactEmail,
            request.ContactPhone,
            request.Address,
            request.Slug,
            ImportServiceConfigId.From(request.ImportServiceConfigId),
            request.Description,
            isActive: true,
            DateTime.UtcNow);

        if (partnerResult.IsFailure)
        {
            return Result.Failure<Guid>(partnerResult.Error);
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _partnerRepository.Add(partnerResult.Value);
            return Result.Success(partnerResult.Value.Id.Value);
        }, cancellationToken);
    }
}
