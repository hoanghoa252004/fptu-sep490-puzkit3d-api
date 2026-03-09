using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.CreatePiece;

internal sealed class CreatePieceCommandHandler : ICommandTHandler<CreatePieceCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IPieceCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreatePieceCommandHandler(
        IInstockProductRepository productRepository,
        IPieceCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreatePieceCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(InstockProductError.NotFound(request.ProductId));
        }

        var partId = PartId.From(request.PartId);
        var part = product.Parts.FirstOrDefault(p => p.Id == partId);

        if (part is null)
        {
            return Result.Failure<Guid>(PartError.NotFound(request.PartId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);

            var pieceResult = Piece.Create(
                code,
                request.Quantity,
                partId);

            if (pieceResult.IsFailure)
            {
                return Result.Failure<Guid>(pieceResult.Error);
            }

            var piece = pieceResult.Value;
            part.AddPiece(piece);
            
            _productRepository.Update(product);

            return Result.Success(piece.Id.Value);
        }, cancellationToken);
    }
}

