using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Commands.UpdatePiece;

internal sealed class UpdatePieceCommandHandler : ICommandHandler<UpdatePieceCommand>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdatePieceCommandHandler(
        IInstockProductRepository productRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePieceCommand request, CancellationToken cancellationToken)
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

        var pieceId = PieceId.From(request.PieceId);
        var piece = part.Pieces.FirstOrDefault(p => p.Id == pieceId);

        if (piece is null)
        {
            return Result.Failure(PieceError.NotFound(request.PieceId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            if (request.NewPartId.HasValue)
            {
                var newPartId = PartId.From(request.NewPartId.Value);
                var newPart = product.Parts.FirstOrDefault(p => p.Id == newPartId);
                
                if (newPart is null)
                {
                    return Result.Failure(PartError.NotFound(request.NewPartId.Value));
                }

                part.RemovePiece(piece);

                var newPieceResult = Piece.Create(
                    piece.Code,
                    request.Quantity,
                    newPartId);

                if (newPieceResult.IsFailure)
                {
                    return Result.Failure(newPieceResult.Error);
                }

                newPart.AddPiece(newPieceResult.Value);
            }
            else
            {
                var updateResult = piece.Update(piece.Code, request.Quantity);

                if (updateResult.IsFailure)
                {
                    return updateResult;
                }
            }
            
            _productRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
    }
}
