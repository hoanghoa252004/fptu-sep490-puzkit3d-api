using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Entities.Pieces;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPieceById;

internal sealed class GetPieceByIdQueryHandler : IQueryHandler<GetPieceByIdQuery, GetPieceByIdResponseDto>
{
    private readonly IInstockProductRepository _productRepository;

    public GetPieceByIdQueryHandler(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResultT<GetPieceByIdResponseDto>> Handle(
        GetPieceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetPieceByIdResponseDto>(
                InstockProductError.NotFound(request.ProductId));
        }

        var partId = PartId.From(request.PartId);
        var part = product.Parts.FirstOrDefault(p => p.Id == partId);

        if (part is null)
        {
            return Result.Failure<GetPieceByIdResponseDto>(
                PartError.NotFound(request.PartId));
        }

        var pieceId = PieceId.From(request.PieceId);
        var piece = part.Pieces.FirstOrDefault(p => p.Id == pieceId);

        if (piece is null)
        {
            return Result.Failure<GetPieceByIdResponseDto>(
                PieceError.NotFound(request.PieceId));
        }

        var response = new GetPieceByIdResponseDto(
            piece.Id.Value,
            piece.Code,
            piece.Quantity,
            piece.PartId.Value);

        return Result.Success(response);
    }
}
