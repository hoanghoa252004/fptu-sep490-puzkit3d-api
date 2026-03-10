using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.UpdatePart;

internal sealed class UpdatePartCommandHandler : ICommandHandler<UpdatePartCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdatePartCommandHandler(
        IInstockProductRepository productRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(InstockProductError.NotFound(request.ProductId));
        }

        var partId = PartId.From(request.PartId);
        var part = product.Parts.FirstOrDefault(p => p.Id == partId);

        if (part is null)
        {
            return Result.Failure(PartError.NotFound(request.PartId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var updateResult = part.Update(request.Name, request.PartType, part.Code);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            _productRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
