using PuzKit3D.Modules.InStock.Application.Data;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.Modules.InStock.Domain.Repositories;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler : ICommandTHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //  ==== Validation with repository ====
        var exists = await _productRepository.ExistsByNameAsync(request.Name, cancellationToken);

        if (exists)
        {
            return Result.Failure<Guid>(ProductError.DuplicateName(request.Name));
        }

        // === MAIN LOGIC ===
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create product using factory method
            var productResult = Product.Create(request.Name, request.Price, request.InitialStock);

            if (productResult.IsFailure)
            {
                return Result.Failure<Guid>(productResult.Error);
            }

            var product = productResult.Value;

            // Add to repository
            _productRepository.Add(product);

            return Result.Success(product.Id.Value);
        }, cancellationToken);
    }
}
