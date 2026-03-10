using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Pieces.Queries.GetPiecesByPartId;

internal sealed class GetPiecesByPartIdQueryHandler 
    : IQueryHandler<GetPiecesByPartIdQuery, IReadOnlyList<GetPiecesByPartIdResponseDto>>
{
    private readonly IInstockProductRepository _productRepository;

    public GetPiecesByPartIdQueryHandler(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResultT<IReadOnlyList<GetPiecesByPartIdResponseDto>>> Handle(
        GetPiecesByPartIdQuery request,
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<IReadOnlyList<GetPiecesByPartIdResponseDto>>(
                InstockProductError.NotFound(request.ProductId));
        }

        var partId = PartId.From(request.PartId);
        var part = product.Parts.FirstOrDefault(p => p.Id == partId);

        if (part is null)
        {
            return Result.Failure<IReadOnlyList<GetPiecesByPartIdResponseDto>>(
                PartError.NotFound(request.PartId));
        }

        var pieces = part.Pieces
            .OrderBy(piece => piece.Code) // Sort by Code ascending
            .Select(piece => new GetPiecesByPartIdResponseDto(
                piece.Id.Value,
                piece.Code,
                piece.Quantity))
            .ToList();

        return Result.Success<IReadOnlyList<GetPiecesByPartIdResponseDto>>(pieces);
    }
}
