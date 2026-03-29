using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.DeletePartner;

internal sealed class DeletePartnerCommandHandler : ICommandHandler<DeletePartnerCommand>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public DeletePartnerCommandHandler(
        IPartnerRepository partnerRepository,
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerRepository = partnerRepository;
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeletePartnerCommand request,
        CancellationToken cancellationToken)
    {
        var partner = await _partnerRepository.GetByIdAsync(
            PartnerId.From(request.Id),
            cancellationToken);

        if (partner is null)
        {
            return Result.Failure(PartnerError.NotFound(request.Id));
        }

        if (!partner.IsActive)
        {
            return Result.Failure(PartnerError.AlreadyInactive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var products = await _partnerProductRepository.FindByPartnerIdAsync(request.Id, cancellationToken);
            foreach (var product in products.Where(p => p.IsActive))
            {
                product.Deactivate();
                _partnerProductRepository.Update(product);
            }

            partner.Deactivate();
            _partnerRepository.Update(partner);

            return Result.Success();
        }, cancellationToken);
    }
}
