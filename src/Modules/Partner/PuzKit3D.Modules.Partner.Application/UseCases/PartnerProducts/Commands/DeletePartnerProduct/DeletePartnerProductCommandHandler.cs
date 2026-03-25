using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.DeletePartnerProduct;

internal sealed class DeletePartnerProductCommandHandler : ICommandHandler<DeletePartnerProductCommand>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public DeletePartnerProductCommandHandler(
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeletePartnerProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _partnerProductRepository.GetByIdAsync(
            PartnerProductId.From(request.Id),
            cancellationToken);

        if (product is null)
        {
            return Result.Failure(PartnerProductError.NotFound(request.Id));
        }

        if (!product.IsActive)
        {
            return Result.Failure(PartnerProductError.AlreadyInactive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            product.Deactivate();
            _partnerProductRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
