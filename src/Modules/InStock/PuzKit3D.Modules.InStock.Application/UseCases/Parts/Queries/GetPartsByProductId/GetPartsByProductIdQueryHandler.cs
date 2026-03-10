using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartsByProductId;

internal sealed class GetPartsByProductIdQueryHandler 
    : IQueryHandler<GetPartsByProductIdQuery, IReadOnlyList<GetPartsByProductIdResponseDto>>
{
    private readonly IInstockProductRepository _productRepository;

    public GetPartsByProductIdQueryHandler(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResultT<IReadOnlyList<GetPartsByProductIdResponseDto>>> Handle(
        GetPartsByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<IReadOnlyList<GetPartsByProductIdResponseDto>>(
                InstockProductError.NotFound(request.ProductId));
        }

        var parts = product.Parts
            .OrderBy(p => p.Code) // Sort by Code ascending
            .Select(p => new GetPartsByProductIdResponseDto(
                p.Id.Value,
                p.Name,
                p.PartType,
                p.Code,
                p.Pieces.Sum(piece => piece.Quantity)))
            .ToList();

        return Result.Success<IReadOnlyList<GetPartsByProductIdResponseDto>>(parts);
    }
}
