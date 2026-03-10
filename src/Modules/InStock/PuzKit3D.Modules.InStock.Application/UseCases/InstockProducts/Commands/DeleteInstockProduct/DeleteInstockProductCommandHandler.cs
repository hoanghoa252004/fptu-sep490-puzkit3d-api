using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.DeleteInstockProduct;

internal sealed class DeleteInstockProductCommandHandler : ICommandHandler<DeleteInstockProductCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public DeleteInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteInstockProductCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.Id);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(InstockProductError.NotFound(request.Id));
        }

        if (!product.IsActive)
        {
            return Result.Failure(InstockProductError.AlreadyInactive(request.Id));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            product.Deactivate();
            _productRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
