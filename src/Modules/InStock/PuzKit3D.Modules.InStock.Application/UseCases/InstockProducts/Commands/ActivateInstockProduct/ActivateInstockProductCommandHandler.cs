using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.ActivateInstockProduct;

internal sealed class ActivateInstockProductCommandHandler : ICommandHandler<ActivateInstockProductCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public ActivateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateInstockProductCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(InstockProductError.NotFound(request.Id));
        }

        if (product.IsActive)
        {
            return Result.Failure(InstockProductError.AlreadyActive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            product.Activate();
            _productRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
