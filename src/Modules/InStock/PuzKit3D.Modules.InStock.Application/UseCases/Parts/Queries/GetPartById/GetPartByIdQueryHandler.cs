using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.Parts.Queries.GetPartById;

internal sealed class GetPartByIdQueryHandler : IQueryHandler<GetPartByIdQuery, GetPartByIdResponseDto>
{
    private readonly IInstockProductRepository _productRepository;

    public GetPartByIdQueryHandler(IInstockProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ResultT<GetPartByIdResponseDto>> Handle(
        GetPartByIdQuery request,
        CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdWithPartsAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<GetPartByIdResponseDto>(
                InstockProductError.NotFound(request.ProductId));
        }

        var partId = PartId.From(request.PartId);
        var part = product.Parts.FirstOrDefault(p => p.Id == partId);

        if (part is null)
        {
            return Result.Failure<GetPartByIdResponseDto>(
                PartError.NotFound(request.PartId));
        }

        var response = new GetPartByIdResponseDto(
            part.Id.Value,
            part.Name,
            part.PartType.ToString(),
            part.Code,
            part.Quantity);

        return Result.Success(response);
    }
}
