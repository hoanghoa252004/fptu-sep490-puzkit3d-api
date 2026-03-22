using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.PatchPartnerProduct;

internal sealed class PatchPartnerProductCommandHandler : ICommandHandler<PatchPartnerProductCommand>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public PatchPartnerProductCommandHandler(
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        PatchPartnerProductCommand request,
        CancellationToken cancellationToken)
    {
        var productId = PartnerProductId.From(request.Id);
        var product = await _partnerProductRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(PartnerProductError.NotFound(request.Id));
        }

        if (product.IsActive)
        {
            return Result.Failure(PartnerProductError.AlreadyActive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            product.Activate();
            _partnerProductRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
