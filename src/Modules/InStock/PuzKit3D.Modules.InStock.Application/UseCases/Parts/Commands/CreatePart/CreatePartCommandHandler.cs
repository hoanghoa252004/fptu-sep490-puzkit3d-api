using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.CreatePart;

internal sealed class CreatePartCommandHandler : ICommandTHandler<CreatePartCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IPartCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreatePartCommandHandler(
        IInstockProductRepository productRepository,
        IPartCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(InstockProductError.NotFound(request.ProductId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            var partResult = Part.Create(
                request.Name,
                request.PartType,
                code,
                productId);

            if (partResult.IsFailure)
            {
                return Result.Failure<Guid>(partResult.Error);
            }

            var part = partResult.Value;
            product.AddPart(part);
            
            _productRepository.Update(product);

            return Result.Success(part.Id.Value);
        }, cancellationToken);
    }
}

